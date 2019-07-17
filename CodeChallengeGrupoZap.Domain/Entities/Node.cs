using System;

namespace CodeChallengeGrupoZap.Domain.Entities
{
    public class Node : INode
    {
        public Func<Immobile, bool> Condition { get; set; }
        public INode Next { get; set; }

        public Node(Func<Immobile, bool> condition, INode next)
        {
            Condition = condition;
            Next = next;
        }

        public bool Filter(Immobile immobile)
        {
            if(Condition(immobile))
            {
                return Next != null ? Next.Filter(immobile) : true;
            }
            else
            {
                return false;
            }
        }
    }
}