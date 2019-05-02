using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using Skidor4.Models;
using Skidor4.Data;
using System.Web.Mvc;
using Skidor4.Models.ViewModels;

namespace Skidor4.Data
{
    public class DbPersonOperation
    {
        db4Entities dbcalls = new db4Entities();

        // <----------------------------------------------------->
        #region ENDAST METODER FÖR ATT HÄMTA FRÅN DATABASEN
        //metod för att hämta alla personer i databasen
        public List<persons> GetPersons()
        {
            return dbcalls.persons.ToList();
        }
        //metod för att hämta personer med filtrering på epost
        public List<persons> GetPersonsByEmailSearch(string searchInput)
        {
            return dbcalls.persons.Where(p => p.email.StartsWith(searchInput)).ToList();
        }
        //Metod för att hämta alla publika personer
        public List<persons> GetPublicPersons()
        {
            return dbcalls.persons.Where(p => p.@public == true).ToList();
        }
        //metod för att hämta en specifik person ur databasen
        public persons getPersonById(int id)
        {
            var result = dbcalls.persons.FirstOrDefault(i => i.pk_id == id);
            return result;
        }
        //Hämta person utifrån chipId
        public chiplink GetPersonByChip(int id)
        {
            var result = dbcalls.chiplink.FirstOrDefault(i => i.chip_id == id);

            return result;
        }

        //metod för att hämta en specifik person ur databasen
        public persons GetPersonByAliasAndPass(string alias, string password)
        {
            var result = dbcalls.persons.FirstOrDefault(i => i.alias == alias && i.password == password);
            return result;
        }

        //metod för att hämta en specifik roll
        public permissions GetRole(persons p)
        {
            var result = dbcalls.permissions.FirstOrDefault(i => i.pk_id == p.fk_permission_id);
            return result;
        }
        //Metod för att hämta alla publika personer
        public persons GetPerson()
        {
            return dbcalls.persons.FirstOrDefault();
        }
        #endregion
        // <----------------------------------------------------->

        // <----------------------------------------------------->
        #region ENDAST METODER FÖR ATT LÄGGA TILL I DATABASEN
        //metod för att lägga till personer i databasen
        public persons Addpersons(persons newperson)
        {
           
            dbcalls.persons.Add(newperson);
            dbcalls.SaveChanges();

            return newperson;
        }
        //metod för att uppdatera personer i databasen
        public persons UpdatePerson(persons person)
        {
            var p = dbcalls.persons.Single(x => x.pk_id == person.pk_id);
            p.firstname = person.firstname;
            p.lastname = person.lastname;
            p.password = person.password;
            p.zipcode = person.zipcode;
            p.city = person.city;
            p.alias = person.alias;
            p.adress = person.adress;
            p.email = person.email;
            p.gender = person.gender;
            p.@public = person.@public;

            dbcalls.SaveChanges();

            return person;
        }

        //Metod för att uppdatera personer
        public CreateUserViewModel UpdatePersonAdmin(CreateUserViewModel per)
        {
            var p = dbcalls.persons.Single(x => x.pk_id == per.Person.pk_id);
            p.firstname = per.Person.firstname;
            p.lastname = per.Person.lastname;
            p.password = per.Person.password;
            p.zipcode = per.Person.zipcode;
            p.city = per.Person.city;
            p.alias = per.Person.alias;
            p.adress = per.Person.adress;
            p.email = per.Person.email;
            p.gender = per.Person.gender;
            p.@public = per.Person.@public;
            p.fk_permission_id = per.chosenPermission;

            dbcalls.SaveChanges();

            return per;
        }
        #endregion
        // <----------------------------------------------------->

        // <----------------------------------------------------->
        #region ENDAST METODER FÖR ATT KOLLA UPP I DATABASEN
        // En metod för att kolla om användaren finns i databasen
        public bool UserCheck(string alias, string password)
        {
            var person = dbcalls.persons.Where(x => x.alias == alias && x.password == password);
            if (person.Any())

                return true;
            else
                return false;
        }

        public bool CheckUserRole(string alias, string rname)
        {
            var role = dbcalls.persons.Where(x => x.alias.Equals(alias)).Include(x => x.permissions.rolename).Where(x => x.permissions.rolename.Equals(rname));
            return role.Any();
        }


        //// Metod för att kolla vilken behörighet användaren har admin
        //public bool PermissionCheck(string alias, string password)
        //{
        //    var role = dbcalls.chiplink.Where(x => x.chip_id == chipId && x.persons.password == password && x.persons.fk_permission_id == 5).Select(x => x.persons.fk_permission_id);

        //    return role.Any();
        //}

        #endregion
        // <----------------------------------------------------->

    }
}