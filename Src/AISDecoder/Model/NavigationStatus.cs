namespace AISDecoder.Model
{
    public enum NavigationStatus
    {
        UnderWayUsingEngine = 0,
        AtAnchor = 1,
        NotUnderCommand = 2,
        RestrictedManoeurability = 3,
        ConstrainedByHerDraught = 4,
        Moored = 5,
        Aground = 6,
        EngagedInFishing = 7,
        UnderWaySailing = 8,
        ReservedHsc = 9,
        ReservedWig = 10,
        Reserved1 = 11,
        Reserved2 = 12,
        Reserved3 = 13,
        AisSartActive = 14,
        NotDefined = 15
    }
}
