using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.UDT;
using System.Xml.Linq;

namespace K12.Sports.FitnessInput
{
    [TableName("ischool_fitness_input_date")]
    public class FitnessInputDateRecord : ActiveRecord
    {
        [Field(Field = "grade_year", Indexed = true)]
        public int GradeYear { get; set; }

        [Field(Field = "start_time", Indexed = false)]
        public DateTime StartTime { get; set; }

        [Field(Field = "end_time", Indexed = false)]
        public DateTime EndTime { get; set; }
    }
}
