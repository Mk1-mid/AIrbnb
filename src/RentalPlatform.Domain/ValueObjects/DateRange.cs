namespace RentalPlatform.Domain.ValueObjects;

public sealed class DateRange
{
    public DateTime CheckIn { get; }
    public DateTime CheckOut { get; }

    public DateRange(DateTime checkIn, DateTime checkOut)
    {
        CheckIn = checkIn.Date.AddHours(14);
        CheckOut = checkOut.Date.AddHours(12);

        if (CheckOut <= CheckIn)
            throw new ArgumentException("CheckOut must be after CheckIn");
    }

    public bool Overlaps(DateRange other) =>
        CheckIn < other.CheckOut && CheckOut > other.CheckIn;

    public int NightCount =>
        (int)(CheckOut.Date - CheckIn.Date).TotalDays;
}
