using DgBooksDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DgBooksDetails.DgBServices
{
    public class generoServises
    {
        public List<Genero> GetGeneros()
        {
            List<Genero> generos = new List<Genero>();

            try
            {
                using (var context = new DgBooksEntities())
                {
                    generos = context.Genero.ToList();
                }
            }
            catch
            {

            }

            return generos;
        }

        public Genero GetGeneroById(int id)
        {
            Genero genero = new Genero();

            try
            {
                using (var context = new DgBooksEntities())
                {
                    genero = context.Genero.FirstOrDefault(p => p.intIdGenero == id);
                }
            }
            catch
            {

            }
            return genero;
        }
    }
}
