using Newtonsoft.Json;
using Serilog;
using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using Validus.OPR.DB.Models;

namespace Validus.OPR.DB
{
    public class OprInitialiser : SqliteCreateDatabaseIfNotExists<OprContext> // SqliteDropCreateDatabaseAlways<OprContext>
    {
        public OprInitialiser(DbModelBuilder builder) 
            : base(builder)
        {
            builder.Entity<Opr>()
                .HasOptional(o => o.DataQualityReport)
                .WithRequired(o => o.Opr);

            builder.Entity<Opr>()
                .HasOptional(o => o.TotalInsuredReport)
                .WithRequired(o => o.Opr);

            builder.Entity<Opr>()
                .HasOptional(o => o.SubscribeDataEntryReport)
                .WithRequired(o => o.Opr);

            builder.Entity<Opr>()
                .HasOptional(o => o.CommentHistory)
                .WithRequired(o => o.Opr);

            builder.Entity<CommentHistory>()
                .HasMany(o => o.Comments)
                .WithRequired(o => o.CommentHistory);
        }

        private string GetJson(string file)
        {
            var assembly = typeof(OprInitialiser).Assembly;

            var name = assembly.GetManifestResourceNames().FirstOrDefault(o => o.Contains(file));

            if (!string.IsNullOrEmpty(name))
            {
                using (var stream = new StreamReader(assembly.GetManifestResourceStream(name)))
                {
                    return stream.ReadToEnd();
                }
            }

            return null;
        }

        protected override void Seed(OprContext context)
        {
            Log.Verbose("OPRInitialiser - Seed");

            const string seed = @"SEED\JsonData";

            var now = DateTime.Now;

            var json = new
            {
                dq = GetJson("DataQuality-012016"),
                ti = GetJson("TotalInsured-012016"),
                sde = GetJson("SubscribeDataEntry-012016")
            };

            var collections = new
            {
                dq = JsonConvert.DeserializeObject<ICollection<DataQuality>>(json.dq),
                ti = JsonConvert.DeserializeObject<ICollection<TotalInsured>>(json.ti),
                sde = JsonConvert.DeserializeObject<ICollection<SubscribeDataEntry>>(json.sde)
            };

            context.CreateReport(new DataQualityReport
            {
                ReportDate = now.Date,
                DataQualities = collections.dq
            }, new TotalInsuredReport
            {
                ReportDate = now.Date,
                TotalInsureds = collections.ti
            }, new SubscribeDataEntryReport
            {
                ReportDate = now.Date,
                SubscribeDataEntries = collections.sde
            }, null, createdBy: seed);
        }
    }
}