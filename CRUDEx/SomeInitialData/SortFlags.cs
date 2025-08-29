namespace CRUDEx.SomeInitialData
{
    public class SortFlags
    {
        public bool NameSortAsc { get; set; } = true;
        public bool EmailSortAsc { get; set; } = true;
        public bool ageSortAsc { get; set; } = true;
        public bool CountrySortAsc { get; set; } = true;
        public bool GenderSortAsc { get; set; } = true;

        public bool GetProp(string name)
        {
            switch (name)
            {
                case ("Name"):
                    return NameSortAsc;
                    break;

                case ("Email"):
                    return EmailSortAsc;
                    break;

                case ("Gender"):
                    return GenderSortAsc;
                    break;


                case ("age"):
                    return ageSortAsc;
                    break;

                case ("Country"):
                    return CountrySortAsc;
                    break;
                default: return true;

            }
        }
        public void SetProp(string name)
        {
            switch (name)
            {
                case ("Name"):
                    NameSortAsc = !NameSortAsc;
                    break;

                case ("Email"):
                    EmailSortAsc = !EmailSortAsc;
                    break;

                case ("Gender"):
                    GenderSortAsc = !GenderSortAsc;
                    break;


                case ("age"):
                    ageSortAsc = !ageSortAsc;
                    break;

                case ("Country"):
                    CountrySortAsc = !CountrySortAsc;
                    break;


            }
        }
    }
}
