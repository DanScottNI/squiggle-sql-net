using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace squiggle.tests
{
    [TestFixture]
    public class Example011
    {
        [Test]
        public void Cascading()
        {
            Table cal_inspection_table = new Table("dbo.CAL_Inspection_table", "cal");
            Table cal_events = new Table("dbo.CAL_Events", "ce");
            Table cal_phase = new Table("dbo.Cal_Phase", "cp");

            SelectQuery select = new SelectQuery(); // base table

            select.addColumn(cal_inspection_table, "*");

            select.addJoin(JoinType.Inner, cal_inspection_table, "inspect_code", cal_events, "cal_type");
            select.addJoin(JoinType.Inner, cal_events, "cal_phase_code", cal_phase, "phase_id");



            Assert.That(Matcher.Normalize(select.ToString()), Is.EqualTo(Matcher.Normalize((
                    @"SELECT     cal.*
FROM         dbo.CAL_Inspection_table AS cal INNER JOIN
                      dbo.CAL_Events AS ce ON cal.Inspect_code = ce.Cal_type INNER JOIN
                      dbo.Cal_Phase AS cp ON ce.Cal_Phase_code = cp.phase_id"))));
        }

        [Test]
        public void SameTable()
        {
            Table insp_inspectors = new Table("dbo.insp_inspectors", "i");
            Table towns = new Table("dbo.towns", "t");
            Table counties = new Table("dbo.counties", "c");

            SelectQuery select = new SelectQuery(); // base table

            select.addColumn(insp_inspectors, "*");

            select.addJoin(JoinType.Inner, insp_inspectors, "town_id", towns, "town_id");
            select.addJoin(JoinType.Inner, insp_inspectors, "county_id", counties, "county_id");

            Assert.That(Matcher.Normalize(select.ToString()), Is.EqualTo(Matcher.Normalize(
                   @"SELECT     i.*
FROM         dbo.INSP_Inspectors AS i INNER JOIN
                      dbo.Towns AS t ON i.Town_ID = t.Town_ID INNER JOIN
                      dbo.Counties AS c ON i.County_ID = c.County_ID")));
        }

        [Test]
        public void SelfReferencial()
        {
            Table cal_inspection_table = new Table("dbo.CAL_Inspection_table", "cal");
            Table parent_insp = new Table("dbo.cal_inspection_table", "parent_insp");

            SelectQuery select = new SelectQuery(); // base table

            select.addColumn(cal_inspection_table, "*");

            select.addJoin(JoinType.LeftOuterJoin, cal_inspection_table, "original_insp_id", parent_insp, "insp_id");

            Assert.That(Matcher.Normalize(select.ToString()), Is.EqualTo(Matcher.Normalize((
                    @"SELECT     cal.*
FROM         dbo.CAL_Inspection_table AS cal LEFT OUTER JOIN
                      dbo.CAL_Inspection_table AS parent_insp ON cal.original_insp_id = parent_insp.Insp_ID"))));
        }

        [Test]
        public void ComplexJoin()
        {
            Table mars_proforma_type = new Table("dbo.mars_proforma_type", "rp");
            Table mars_proforma_subj = new Table("dbo.mars_proforma_subj", "s");
            Table mars_proforma = new Table("dbo.mars_proforma", "pro");
            Table mars_proforma_legacy_fe = new Table("dbo.mars_proforma_legacy_fe", "insp");
            Table mars_proforma_subj_map = new Table("dbo.mars_proforma_subj_map", "map");

            SelectQuery select = new SelectQuery();

            select.addColumn(mars_proforma, "mars_proforma_id");
            select.addColumn(mars_proforma_legacy_fe, "leadership_management", "leadership_man");
            select.addColumn(null, null, "how_effective_leadership");
            select.addColumn(mars_proforma_legacy_fe, "leadership_comments");
            select.addColumn(mars_proforma_type, "database_name");
            select.addColumn(mars_proforma_type, "proforma_type_name");
            select.addColumn(mars_proforma_type, "mars_proforma_type_id");
            select.addColumn(mars_proforma, "date_updated");

            select.addJoin(JoinType.Inner, mars_proforma, "mars_proforma_id", mars_proforma_legacy_fe, "mars_proforma_id");
            select.addJoin(JoinType.Inner, mars_proforma, "subj_id", mars_proforma_subj_map, "chl_proforma_subj_id");
            select.addJoin(JoinType.Inner, mars_proforma_subj_map, "parent_proforma_subj_id", mars_proforma_subj, "subj_id");
            select.addJoin(JoinType.Inner, mars_proforma_subj, "mars_proforma_type_id", mars_proforma_type, "mars_proforma_type_id");

            Assert.That(Matcher.Normalize(select.ToString()), Is.EqualTo(Matcher.Normalize("select pro.mars_proforma_id , insp.leadership_management as leadership_man , null as how_effective_leadership , " +
               "insp.leadership_comments , rp.database_name , rp.proforma_type_name , rp.mars_proforma_type_id , pro.date_updated " +
"from dbo.mars_proforma as pro inner join dbo.mars_proforma_legacy_fe as insp on pro.mars_proforma_id = insp.mars_proforma_id inner join " +
"dbo.mars_proforma_subj_map as map on pro.subj_id = map.chl_proforma_subj_id inner join dbo.mars_proforma_subj as s on map.parent_proforma_subj_id " +
"= s.subj_id inner join dbo.mars_proforma_type as rp on s.mars_proforma_type_id = rp.mars_proforma_type_id")));
        }
    }
}
