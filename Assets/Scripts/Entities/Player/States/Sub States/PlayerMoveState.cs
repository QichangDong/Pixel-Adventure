using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Entity entity, string animBoolName) : base(entity, animBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        core.Movement.CheckIfShouldFlip(xInput);
        //player.particleSystem.Play();

        // if (!isExitingState)
        //     return;
        
        if (xInput == 0)
            stateMachine.ChangeState(player.IdleState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        core.Movement.SetVelocityX(playerData.movementVelocity * xInput);
    }
}
