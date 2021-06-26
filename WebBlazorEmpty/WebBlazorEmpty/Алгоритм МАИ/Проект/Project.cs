﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSSAlternative.AHP
{

    public interface IProject
    {
        event Action UpdatedHierOrRelationChanged;

        string ViewFilter { get; set; }
        bool UnsavedChanged { get; }


        ITemplate Template { get; }
        IHierarchy ProblemEditing { get; }
        IProblem Problem { get; }
        

        IStage StageHier { get; }
        IStage StageView { get; }
        IStage StageResults { get; }

        Dictionary<INodeRelation, IStage> StageRelations { get; }
        IStage GetRelation(INode node);


        void UpdateProblem();
        string Status { get; }
    }
    public class Project : IProject
    {
        public override string ToString() => Problem.ToString();
        public event Action UpdatedHierOrRelationChanged;


        public ITemplate Template { get; private set; }
        public IHierarchy ProblemEditing => new HierarchySheme(Template);
        public IProblem Problem { get; private set; }


        public bool UnsavedChanged => !HierarchySheme.CompareEqual(Problem, ProblemEditing);
        public string Status
        {
            get
            {
                string status = "Готова";
                if (!Problem.CorrectnessRels.AreRelationsKnown)
                    status = "Нужна информация";
                if (!Problem.CorrectnessRels.AreRelationsConsistenct)
                    status = "Нужна корректировка";
                return status;
            }
        }
        public string ViewFilter { get; set; } = "По отношениям";


        public Project(ITemplate template)
        {
            Console.WriteLine("Создание проекта и обновление иерархии");
            SetProblem(template);      
        }
        public void UpdateProblem()
        {
            Console.WriteLine("Обновление иерархии");
            SetProblem(Template);
        }
        private void SetProblem(ITemplate template)
        {
            Template = template;
            IProblem old = Problem;
            Problem = new Problem(Template);
            SetStages();

            if (old != null)
            {
                old.RelationValueChanged -= Update;
            }
            Problem.RelationValueChanged += Update;
            Update();

            void Update()
            {
                UpdatedHierOrRelationChanged?.Invoke();
            }
        }


        private void SetStages()
        {
            StageHier = new StageHierarchy(this);
            StageView = new StageView(this);
            StageResults = new StageResults(this);

            StageRelations.Clear();
            foreach (var relation in Problem.RelationsAll)
            {
                IStage relStage = new StageRelation(this, relation);
                StageRelations.Add(relation, relStage);
            }
        }
        public IStage StageHier { get; private set; }
        public IStage StageView { get; private set; }
        public IStage StageResults { get; private set; }

        public Dictionary<INodeRelation, IStage> StageRelations { get; private set; } = new Dictionary<INodeRelation, IStage>();
        public IStage GetRelation(INode node)
        {
            var relation = Problem.FirstRequiredRelation(node);

            if (relation != null && StageRelations.ContainsKey(relation))
                return StageRelations[relation];
            else
                return StageRelations.First().Value;
        }
    }

}
