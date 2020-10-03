﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSSLib
{
    public class ChoiceCriterias : Choice
    {
        public override string ToString() => $"Решение по критериям: {Problem.Name}";

        //Проверить на наличие у альтернатив всех указанных критериев в выборе
        public static CheckChoiceResult CheckAll(Problem problem)
        {
            CheckChoiceResult check = new CheckChoiceResult();
            if(problem.Alternatives.Count == 0)
            {
                check.Success = false;
                check.Messages.Add("- В проблеме не задано альтернатив, из которых можно выбирать");
                return check;
            }
            if(problem.Cases.Count == 0)
            {
                check.Success = false;
                check.Messages.Add("- В проблеме не заданы исходы, которые определяют судьбу альтернатив");
                return check;
            }
            double sum = problem.Cases.Select(c => c.Chance).Aggregate((now, s) => s += now);
            if(sum < 0.98 || sum > 1)
            {
                check.Success = false;
                check.Messages.Add($"- Сумма вероятностей исходов не равна 1 ({sum})");
            }

            foreach (Alternative alternative in problem.Alternatives)
            {
                foreach (Case caseC in problem.Cases)
                {
                    if(alternative.GetCaseProfit(caseC) == null)
                    {
                        check.Success = false;
                        check.Messages.Add($"- В альтернативе {alternative.Name} отсутствует выгода от исхода {caseC.Name}");
                    }
                }
            }
            return check;
        }
        public static bool IsSolvable(Problem problem) => CheckAll(problem).Success;



        public PayMatrix PayMatrixWins { get; set; }
        public PayMatrix PayMatrixSafe { get; set; }


        public ChoiceCriterias(Problem problem) : base(problem)
        {
            CountDesizion();
        }
        protected override void CountDesizion()
        {
            PayMatrixWins = new PayMatrix(Problem.Alternatives, Problem.Cases,true);
            PayMatrixSafe = new PayMatrix(Problem.Alternatives, Problem.Cases, false);
        }

        public override void Output()
        {
            Console.WriteLine(ToString());
            PayMatrixWins.Output();
            PayMatrixSafe.Output();
        }
    }
}
