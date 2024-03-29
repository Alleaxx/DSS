﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSSCriterias.Logic
{

    public abstract class ReportXAML : Report
    {
        public ReportXAML(IStatGame matrix) : base(matrix)
        {

        }

        public override void Create()
        {
            //Генерируем текст XAML-документа
        }

        public override abstract void Open();
    }
    public class ReportPDF : ReportXAML
    {
        public ReportPDF(IStatGame matrix) : base(matrix)
        {

        }

        public override void Open()
        {
            throw new NotImplementedException();
        }
    }
    public class ReportWord : ReportXAML
    {
        public ReportWord(IStatGame matrix) : base(matrix)
        {

        }

        public override void Open()
        {
            throw new NotImplementedException();
        }
    }
}
