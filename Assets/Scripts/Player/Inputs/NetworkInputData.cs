using Fusion;

public struct NetworkInputData : INetworkInput
{
    public float movementInput;

    public NetworkBool isJumpPressed;

    public NetworkBool isFirePressed;

    public NetworkBool isMeleePressed;
}