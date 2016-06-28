using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Validus.OPR.DB.Models;

namespace Validus.OPR.DB
{
    public class OprContext : DbContext
    {
        protected const string DefaultConnection = "OPR";

        public DbSet<Opr> Opr { get; set; }

        public DbSet<DataQualityReport> DataQualityReport { get; set; }

        public DbSet<DataQuality> DataQuality { get; set; }

        public DbSet<TotalInsuredReport> TotalInsuredReport { get; set; }

        public DbSet<TotalInsured> TotalInsured { get; set; }

        public DbSet<SubscribeDataEntryReport> SubscribeDataEntryReport { get; set; }

        public DbSet<SubscribeDataEntry> SubscribeDataEntry { get; set; }

        public DbSet<CommentHistory> CommentHistory { get; set; }

        public DbSet<Comments> Comments { get; set; }

        public OprContext(string connection = DefaultConnection) 
            : base(connection)
        {
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            Database.SetInitializer(new OprInitialiser(builder));
        }

        public Opr CreateReport(DataQualityReport dqr, TotalInsuredReport tir, SubscribeDataEntryReport sder, CommentHistory ch, string createdBy)
        {
            var now = DateTime.Now;
            var report = new Opr
            {
                DataQualityReport = dqr ?? new DataQualityReport(),
                TotalInsuredReport = tir ?? new TotalInsuredReport(),
                SubscribeDataEntryReport = sder ?? new SubscribeDataEntryReport(),
                CommentHistory = ch ?? new CommentHistory(),
                CreatedBy = createdBy,
                ModifiedBy = createdBy,
                CreatedOn = now,
                ModifiedOn = now
            };

            Opr.Add(report);

            SaveChanges();

            return report;
        }

        public IQueryable<Opr> GetReports()
        {
            return Opr.AsQueryable();
        }

        public IQueryable<Opr> GetReport(long id, bool includeReports = false, bool includeComments = false)
        {
            var report = GetReports().Where(r => r.Id == id);

            if (includeReports)
            {
                report = report
                    .Include(r => r.DataQualityReport)
                    .Include(r => r.DataQualityReport.DataQualities)
                    .Include(r => r.TotalInsuredReport)
                    .Include(r => r.TotalInsuredReport.TotalInsureds)
                    .Include(r => r.SubscribeDataEntryReport)
                    .Include(r => r.SubscribeDataEntryReport.SubscribeDataEntries);
            }

            if (includeComments)
            {
                report = report
                    .Include(r => r.CommentHistory)
                    .Include(r => r.CommentHistory.Comments);
            }

            return report;
        }

        public Comments AddComment(long reportId, string type, string comment, string author, string username)
        {
            var report = GetReport(reportId, includeComments: true).SingleOrDefault();

            if (report != null)
            {
                var now = DateTime.Now;
                var db = new
                {
                    comment = new Comments
                    {
                        Type = type,
                        Comment = comment,
                        Author = author,
                        CreatedBy = username,
                        ModifiedBy = username,
                        CreatedOn = now,
                        ModifiedOn = now,
                        CommentHistory = report.CommentHistory
                    }
                };
                
                Comments.Add(db.comment);

                SaveChanges();

                return db.comment;
            }

            return null;
        }

        public int DeleteComment(long reportId, long id)
        {
            var report = GetReport(reportId).SingleOrDefault();

            if (report != null && report.CommentHistory != null)
            {
                var comment = report.CommentHistory.Comments.Where(c => c.Id == id).SingleOrDefault();

                if (comment != null) Comments.Remove(comment);
            }

            return SaveChanges();
        }
    }
}