using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHSTG.SharedKernel.BaseDomainObjects
{
    public abstract class Subject
    {
        private List<Observer> _observers = new List<Observer>();

        public List<Observer> Observers
        {
            get { return _observers; }
            set { _observers = value; }
        }

        public void Add(Observer observer)
        {
            _observers.Add(observer);
        }

        public void Detach(Observer observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(Entity<int> entityToRemove)
        {
            //Only have one game manager, so Only one observer
            Observer obs = _observers[0];

            obs.Update(entityToRemove);
        }
    }
}
