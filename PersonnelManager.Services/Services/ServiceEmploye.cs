using System;
using System.Collections.Generic;
using System.Linq;
using PersonnelManager.Business.Exceptions;
using PersonnelManager.Dal.Data;
using PersonnelManager.Dal.Entites;

namespace PersonnelManager.Business.Services
{
    public class ServiceEmploye
    {
        private readonly IDataEmploye dataEmploye;

        public ServiceEmploye(IDataEmploye dataEmploye)
        {
            this.dataEmploye = dataEmploye;
        }

        public Ouvrier GetOuvrier(string nom)
        {
            return this.dataEmploye.GetListeOuvriers().FirstOrDefault(x => x.Nom == nom);
        }

        public Ouvrier GetOuvrier(int idOuvrier)
        {
            return this.dataEmploye.GetOuvrier(idOuvrier);
        }

        public Cadre GetCadre(int idCadre)
        {
            return this.dataEmploye.GetCadre(idCadre);
        }

        public void EnregistrerCadre(Cadre cadre)
        {
            if (cadre == null)
            {
                throw new InvalidOperationException();
            }
            if (cadre.DateEmbauche.Year <= 1920)
            {
                throw new BusinessException("La date d'embauche doit être > 1920");
            }
            if (cadre.DateEmbauche > DateTime.Now.AddMonths(3))
            {
                throw new BusinessException("La date d'embauche doit être inférieure à 3 mois à partir d'aujourdhui");

            }

            this.dataEmploye.EnregistrerCadre(cadre);
        }

        public void EnregistrerOuvrier(Ouvrier ouvrier)
        {
            if (ouvrier == null)
            {
                throw new InvalidOperationException();
            }

            if (ouvrier.TauxHoraire <= 0)
            {
                throw new BusinessException("Taux horaire invalide");
            }

            if (ouvrier.DateEmbauche.Year <= 1920)
            {
                throw new BusinessException("La date d'embauche doit être > 1920");
            }

            this.dataEmploye.EnregistrerOuvrier(ouvrier);
        }

        public IEnumerable<Employe> GetListeEmployes()
        {
            var listeEmployes = new List<Employe>();
            listeEmployes.AddRange(this.dataEmploye.GetListeOuvriers());
            listeEmployes.AddRange(this.dataEmploye.GetListeCadres());

            return listeEmployes.OrderBy(x => x.Nom).ThenBy(x => x.Prenom);
        }

        public IEnumerable<SalaireOuvrier> GetSalaireOuvrier(int idOuvrier, DateTime mois)
        {
            return null;
        }

        public IEnumerable<Salaire> GetSalaireCadre(int idCadre, DateTime mois)
        {
            return null;
        }
    }
}
