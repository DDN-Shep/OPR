using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Serilog;
using System.Linq;
using Validus.OPR.DB;

namespace Validus.OPR
{
    public class OprHub : Hub
    {
        public void GetReport(long id)
        {
            Log.Verbose("OprHub - GetReport - id={0}: {0}", id, this.GetUserName());

            using (var db = new OprContext())
            {
                var report = db.GetReport(id, includeReports: true).SingleOrDefault();

                Log.Debug("Report = {0}", JsonConvert.SerializeObject(report));

                Clients.Caller.loadReport(report);
            }
        }

        /// <summary>
        /// Get the latest comments (one for each type)
        /// </summary>
        /// <param name="reportId"></param>
        public void GetLatestComments(long reportId)
        {
            Log.Debug("Socket - GetLatestComments called by {0} with {1}", this.GetUserName(), reportId);

            using (var db = new OprContext())
            {
                var report = db.GetReport(reportId, includeComments: true).SingleOrDefault();

                if (report != null && report.CommentHistory != null)
                {
                    var comments = report.CommentHistory.Comments
                        .OrderByDescending(c => c.ModifiedOn)
                        .GroupBy(c => c.Type, (key, g) => new
                        {
                            Type = key,
                            Comment = g.FirstOrDefault()
                        }).ToArray();

                    Clients.Caller.loadComments(comments);
                }
            }
        }

        /// <summary>
        /// Add a comment. If the comment history does not already exist (id == null), then created it.
        /// </summary>
        /// <param name="type">The comment type identifier</param>
        /// <param name="comment"></param>
        /// <param name="id">The comment history id</param>
        public void AddComment(long reportId, string type, string comment)
        {
            var user = new
            {
                name = this.GetUserName(),
                display = this.GetUserDisplayName()
            };

            Log.Debug("Socket - AddComment called by {0} with {1} for {2}", user.name, comment, type, reportId);

            using (var db = new OprContext())
            {
                var result = db.AddComment(reportId, type, comment, user.display, user.name);

                Clients.All.setComment(type, user.display, comment, result.CreatedOn);
            }
        }
    }
}