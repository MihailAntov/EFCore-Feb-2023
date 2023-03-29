﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType("Task")]
    public class TaskImportDto
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(2),MaxLength(40)]
        public string Name { get; set; }

        [XmlElement("OpenDate")]
        [Required]
        public string OpenDate { get; set; }

        [XmlElement("DueDate")]
        [Required]
        public string DueDate { get; set; }

        [XmlElement("ExecutionType")]
        [Range(0,3)]
        [Required]
        public int ExecutionType { get; set; }

        [XmlElement("LabelType")]
        [Range(0,4)]
        [Required]
        public int LabelType { get; set; }
    }
}

