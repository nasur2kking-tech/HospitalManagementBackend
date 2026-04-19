namespace HospitalManagement.Application.Common;

public class PaginationParams
{
    private const int MaxPageSize = 50;
    private const int MinPageNumber = 1;

    private int _pageNumber = 1;
    private int _pageSize = 10;

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < MinPageNumber ? MinPageNumber : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set
        {
            if (value <= 0)
                _pageSize = 10;
            else if (value > MaxPageSize)
                _pageSize = MaxPageSize;
            else
                _pageSize = value;
        }
    }
}
