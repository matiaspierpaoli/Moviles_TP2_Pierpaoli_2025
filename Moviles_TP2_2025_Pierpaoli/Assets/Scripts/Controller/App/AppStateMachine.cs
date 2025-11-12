using System;
using System.Collections.Generic;

namespace Game.Controller
{
    public interface IAppState
    {
        void Enter();
        void Tick();
        void Exit();
    }

    public class AppStateMachine
    {
        readonly Dictionary<Type, IAppState> map = new();
        IAppState cur;

        public void Register(IAppState s)
        {
            map[s.GetType()] = s;
        }

        public T Get<T>() where T : IAppState => (T)map[typeof(T)];

        public void Change<T>() where T : IAppState
        {
            IAppState nxt = Get<T>();
            if (cur == nxt) return;
            cur?.Exit();
            cur = nxt;
            cur.Enter();
        }
        
        public void Change(Type T)
        {
            if (!map.ContainsKey(T)) return;
            IAppState nxt = map[T];
            if (cur == nxt) return;
            cur?.Exit();
            cur = nxt;
            cur.Enter();
        }
        
        public void Tick()
        {
            cur?.Tick();
        }
    }
}