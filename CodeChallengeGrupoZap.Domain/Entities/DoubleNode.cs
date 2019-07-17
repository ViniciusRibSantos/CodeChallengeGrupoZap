using System;

namespace CodeChallengeGrupoZap.Domain.Entities
{
    public class DoubleNode : INode
    {
        public Func<Immobile, bool> Condition { get; set; }
        public INode Right { get; set; }
        public INode Left { get; set; }

        public DoubleNode(Func<Immobile, bool> condition, INode right, INode left)
        {
            Condition = condition;
            Right = right;
            Left = left;
        }
        public bool Filter(Immobile immobile)
        {
            if(Condition(immobile))
            {
                return Right.Filter(immobile);
            }
            else
            {
                return Left.Filter(immobile);
            }
        }
    }
}