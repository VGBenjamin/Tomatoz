namespace Tomatoz.Shared.Enums;

public enum TomatoGrowthType
{
    Determinate,
    Indeterminate,
    SemiDeterminate
}

public enum TomatoColor
{
    Red,
    Pink,
    Yellow,
    Orange,
    Green,
    Purple,
    Black,
    White,
    Striped,
    Multicolor
}

public enum TomatoShape
{
    Round,
    Elongated,
    Oval,
    Heart,
    Pear,
    Cherry,
    Grape,
    Beefsteak,
    Roma,
    Plum
}

public enum TomatoSize
{
    Micro,
    Cherry,
    Cocktail,
    Medium,
    Large,
    Beefsteak,
    Giant
}

public enum TomatoTexture
{
    Firm,
    Soft,
    Juicy,
    Meaty,
    Crispy
}

public enum TomatoFleshType
{
    Regular,
    Dense,
    Hollow,
    Filled
}

public enum DiseaseResistance
{
    None,
    Low,
    Moderate,
    High,
    VeryHigh
}

public enum CultivationSeason
{
    Spring,
    Summer,
    Fall,
    Winter,
    AllYear
}

public enum ContributionStatus
{
    Draft,
    Proposed,
    UnderReview,
    Approved,
    Rejected,
    Published
}

public enum UserRole
{
    Visitor,
    User,
    Contributor,
    Moderator,
    Administrator
}

public enum VoteType
{
    Taste,
    EaseOfGrowing,
    Productivity,
    Appearance,
    Overall,
    Utility,
    Quality,
    Accuracy
}

public enum ActivityType
{
    CreateVariety,
    EditVariety,
    AddPhoto,
    AddComment,
    Vote,
    Follow,
    Approve,
    Reject
}

public enum PhotoType
{
    Plant,
    Fruit,
    Slice,
    Seeds,
    Harvest,
    Flower,
    Leaf,
    Garden,
    Other
}
