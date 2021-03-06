using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
   
    //Input
    private int xInput;
    private bool jumpInput;
    private bool jumpInputStop;
    private bool grabInput;
    private bool dashInput;

    //Checks
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingWallBack;
    private bool oldIsTouchingWall;
    private bool oldIsTouchingWallBack;
    private bool isTouchingLedge;

    private bool coyoteTime;
    private bool wallJumpCoyoteTime;
    private bool isJumping;
    private bool doubleJump;

    private float startWallJumpCoyoteTime;

    public PlayerInAirState(Player entity, string animBoolName) : base(entity, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        oldIsTouchingWall = isTouchingWall;
        oldIsTouchingWallBack = isTouchingWallBack;

        isGrounded = core.CollisionSenses.Ground;
        isTouchingWall = core.CollisionSenses.WallFront;
        isTouchingWallBack = core.CollisionSenses.WallBack;
        // isTouchingLedge = core.CollisionSenses.LedgeHorizontal;


        if(!wallJumpCoyoteTime && !isTouchingWall && !isTouchingWallBack && (oldIsTouchingWall || oldIsTouchingWallBack))
        {
            StartWallJumpCoyoteTime();
        }
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        oldIsTouchingWall = false;
        oldIsTouchingWallBack = false;
        isTouchingWall = false;
        isTouchingWallBack = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();

        xInput = player.InputHandler.NormInputX;
        jumpInput = player.InputHandler.JumpInput;
        jumpInputStop = player.InputHandler.JumpInputStop;
        grabInput = player.InputHandler.GrabInput;
        dashInput = player.InputHandler.DashInput;


        if (isGrounded && core.Movement.CurrentVelocity.y < 0.01f)
            stateMachine.ChangeState(player.LandState);
        // else if(jumpInput && (isTouchingWall || isTouchingWallBack || wallJumpCoyoteTime))
        // {
        //     StopWallJumpCoyoteTime();
        //     isTouchingWall = core.CollisionSenses.WallFront;
        //     player.WallJumpState.DetermineWallJumpDirection(isTouchingWall);
        //     stateMachine.ChangeState(player.WallJumpState);
        // }
        else if(jumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        }
        // else if(isTouchingWall && grabInput && isTouchingLedge)
        // {
        //     stateMachine.ChangeState(player.WallGrabState);
        // }
        else if(isTouchingWall && core.Movement.CurrentVelocity.y <= 1 && xInput != -core.Movement.FacingDirection) /*xInput == core.Movement.FacingDirection &&*/
        {
            player.JumpState.Resetjumps();
            stateMachine.ChangeState(player.WallSlideState);
        }
        // else if(dashInput && player.DashState.CheckIfCanDash())
        // {
        //     stateMachine.ChangeState(player.DashState);
        // }
        else
        {
            core.Movement.CheckIfShouldFlip(xInput);
            player.Anim.SetFloat("yVelocity", core.Movement.CurrentVelocity.y);
        }

    }

    private void CheckJumpMultiplier()
    {
        if (isJumping)
        {
            if (jumpInputStop)
            {
                core.Movement.SetVelocityY(core.Movement.CurrentVelocity.y * playerData.variableJumpHeightMultiplier);
                isJumping = false;
            }
            else if (core.Movement.CurrentVelocity.y <= 0f)
            {
                isJumping = false;
            }

        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if(isExitingState)
            return;

        // CheckJumpMultiplier();
        core.Movement.SetVelocityX(playerData.movementVelocity * xInput);
    }

    private void CheckCoyoteTime()
    {
        if(coyoteTime && Time.time > startTime + playerData.coyoteTime)
        {
            coyoteTime = false;
            player.JumpState.UseJump();
        }
    }

    private void CheckWallJumpCoyoteTime()
    {
        if(wallJumpCoyoteTime && Time.time > startWallJumpCoyoteTime + playerData.coyoteTime)
        {
            wallJumpCoyoteTime = false;
        }
    }

    public void StartCoyoteTime() => coyoteTime = true;

    public void StartWallJumpCoyoteTime()
    {
        wallJumpCoyoteTime = true;
        startWallJumpCoyoteTime = Time.time;
    }

    public void StopWallJumpCoyoteTime() => wallJumpCoyoteTime = false;

    public void SetIsJumping() => isJumping = true;
}

