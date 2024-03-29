﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSSLib
{
    interface IAlternative
    {
        string Name { get; set; }
        string Description { get; set; }
    }

    public class Alternative : NotifyObj
    {
        public override string ToString() => Name;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        private string name;


        public string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged();
            }
        }
        private string description;

        public string Image
        {
            get => image;
            set
            {
                image = value;
                OnPropertyChanged();
            }
        }
        private string image;


        public Alternative() : this("") { }
        public Alternative(string name)
        {
            Name = name;
        }
    }
}
