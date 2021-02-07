namespace Combine
{
    public class NotWalkableCell : Cell
    {
        public override CellType Type => CellType.NotWalkable;


        public override void PlayerCollisionEnter(PlayerCharacter player)
        {
            // Check side of collision.
            CollisionSide side = CollisionSideHelper.Determine(transform.position, player.transform.position);

            player.Die(side);
        }

        public override void PursuerCollisionEnter(Pursuer pursuer)
        {
            pursuer.HitNotWalkable();
        }
    }
}