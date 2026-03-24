using System;

namespace _Project.Develop.Runtime.Utilities.Conditions
{
    public class FuncCondition : ICondition
    {
        private readonly Func<bool> _condition;

        public FuncCondition(Func<bool> condition)
        {
            _condition = condition;
        }

        public bool Evaluate() => _condition.Invoke();
    }
}
