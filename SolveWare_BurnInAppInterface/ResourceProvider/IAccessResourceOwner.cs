namespace SolveWare_BurnInAppInterface
{
    public interface IAccessResourceOwner
    {
        bool CanCurrentOwnerAccessResource(GenernalResourceOwner currnetResourceOwner, string resourceOwnerName);
    }
}