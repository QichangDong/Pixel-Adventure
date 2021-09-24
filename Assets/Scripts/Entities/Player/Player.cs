using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachine;

public class Player : Entity
{    

    public PlayerInputHandler InputHandler { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    // public PlayerWallGrabState WallGrabState { get; private set; }
    // public PlayerWallClimbState WallClimbState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    // public PlayerLedgeClimbState LedgeClimbState { get; private set; }
    // public PlayerDashState DashState { get; private set; }

    public override void Awake()
    {
        base.Awake();

        StateMachine = new FSM();

        IdleState = new PlayerIdleState(this, "idle");
        MoveState = new PlayerMoveState(this, "move");
        JumpState = new PlayerJumpState(this, "inAir");
        InAirState = new PlayerInAirState(this, "inAir");
        LandState = new PlayerLandState(this, "idle");
        WallSlideState = new PlayerWallSlideState(this, "wallSlide");
        // WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, "wallGrab");
        // WallClimbState = new PlayerWallClimbState(this, StateMachine, playerData, "wallClimb");
        WallJumpState = new PlayerWallJumpState(this, "inAir");
        // DashState = new PlayerDashState(this, StateMachine, playerData, "inAir");
        // AttackState = new PlayerAttackState(this, StateMachine, playerData, "attack");
    }

    private void Start()
    {
        InputHandler = GetComponent<PlayerInputHandler>();
        //Inventory = GetComponent<PlayerInventory>();

        //AttackState.SetWeapon(Inventory.weapons[(int)CombatInputs.primary]);
        StateMachine.Initialize(IdleState);
    }

     private void Update()
    {
        Core.LogicUpdate();
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
}