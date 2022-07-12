namespace SAMU192iOS.FacadeStub
{
    public static class StubWalkThrough
    {
        static bool InWalkThrough = false;

        public static bool GetInWalkThrough()
        {
            return InWalkThrough;
        }

        public static void SetInWalkThrough(bool inWalkThrough)
        {
            InWalkThrough = inWalkThrough;
        }
    }
}