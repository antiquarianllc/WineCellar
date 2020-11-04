


namespace WineCellar.Helpers
{
    using WineCellar.Entities;
    using WineCellar.Models;

    public class VarietalConverter
    {
        private WineCellarDBContext _context;

        public VarietalConverter( WineCellarDBContext context ) => _context = context;

        public VarietalEntity Convert( WineVarietal source )
        {
            VarietalEntity destination = _context.Varietals.Find( source.Id );

            if (destination == null)
            {
                destination = new VarietalEntity( );
            }

            destination.Id = source.Id;
            destination.Varietal = source.Varietal;

            return destination;
        }

        public WineVarietal Convert( VarietalEntity source )
        {

            WineVarietal destination = new WineVarietal( )
            {
                Id = source.Id,
                Varietal = source.Varietal
            };

            return destination;
        }
    }
}



/*

public class PersonConverter
{
    public MyDatabaseContext _db;

    public PersonEntity Convert(PersonModel source)
    {
         PersonEntity destination = _db.People.Find(source.ID);

         if(destination == null)
             destination = new PersonEntity();

         destination.Name = source.Name;
         destination.Organisation = _db.Organisations.Find(source.OrganisationID);
         //etc

         return destination;
    }

    public PersonModel Convert(PersonEntity source)
    {
         PersonModel destination = new PersonModel()
         {
             Name = source.Name,
             OrganisationID = source.Organisation.ID,
             //etc
         };

         return destination;
    }
}

*/