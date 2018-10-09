using System.Web.Mvc;
using PersonnelManager.Business.Exceptions;
using PersonnelManager.Business.Services;
using PersonnelManager.Dal.Data;
using PersonnelManager.Dal.Entites;
using PersonnelManager.Filters;
using PersonnelManager.Models;

namespace PersonnelManager.Controllers
{
    [Authentication(Type = TypeUtilisateur.Manager)]
    public class EmployesController : Controller
    {
        private readonly ServiceEmploye serviceEmploye;

        public EmployesController()
        {
            this.serviceEmploye = new ServiceEmploye(new DbDataEmploye());
        }

        public ActionResult Index()
        {
            var employes = this.serviceEmploye.GetListeEmployes();
            return this.View("ListeEmployes", employes);
        }

        public ActionResult NouvelOuvrier()
        {
            return this.View("EditionOuvrier", new EditionOuvrierViewModel());
        }

        [HttpPost]
        public ActionResult NouvelOuvrier(EditionOuvrierViewModel viewModel)
        {
            return this.EnregistrerOuvrier(viewModel);
        }

        public ActionResult EditionOuvrier(int idOuvrier)
        {
            var ouvrier = this.serviceEmploye.GetOuvrier(idOuvrier);
            var viewModel = new EditionOuvrierViewModel
            {
                Id = ouvrier.Id,
                DateEmbauche = ouvrier.DateEmbauche,
                Nom = ouvrier.Nom,
                Prenom = ouvrier.Prenom,
                TauxHoraire = ouvrier.TauxHoraire
            };
            return this.View("EditionOuvrier", viewModel);
        }

        [HttpPost]
        public ActionResult EditionOuvrier(EditionOuvrierViewModel viewModel)
        {
            return this.EnregistrerOuvrier(viewModel);
        }

        public ActionResult NouveauCadre()
        {
            return this.View("EditionCadre", new EditionCadreViewModel());
        }

        [HttpPost]
        public ActionResult NouveauCadre(EditionCadreViewModel viewModel)
        {
            return this.EnregistrerCadre(viewModel);
        }

        public ActionResult EditionCadre(int idCadre)
        {
            var cadre = this.serviceEmploye.GetCadre(idCadre);
            var viewModel = new EditionCadreViewModel
            {
                Id = cadre.Id,
                DateEmbauche = cadre.DateEmbauche,
                Nom = cadre.Nom,
                Prenom = cadre.Prenom,
                SalaireMensuel = cadre.SalaireMensuel
            };
            return this.View("EditionCadre", viewModel);
        }

        [HttpPost]
        public ActionResult EditionCadre(EditionCadreViewModel viewModel)
        {
            return this.EnregistrerCadre(viewModel);
        }

        private ActionResult EnregistrerCadre(EditionCadreViewModel viewModel)
        {
            if (this.ModelState.IsValid)
            {
                var cadre = viewModel.Id.HasValue
                    ? this.serviceEmploye.GetCadre(viewModel.Id.Value)
                    : new Cadre();
                viewModel.UpdateCadre(cadre);
                try
                {
                    this.serviceEmploye.EnregistrerCadre(cadre);
                    return this.RedirectToAction("Index", "Employes");
                }
                catch (BusinessException exception)
                {
                    this.ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return this.View("EditionCadre", viewModel);
        }

        private ActionResult EnregistrerOuvrier(EditionOuvrierViewModel viewModel)
        {
            if (this.ModelState.IsValid)
            {
                var ouvrier = viewModel.Id.HasValue
                    ? this.serviceEmploye.GetOuvrier(viewModel.Id.Value)
                    : new Ouvrier();

                viewModel.UpdateOuvrier(ouvrier);
                try
                {
                    this.serviceEmploye.EnregistrerOuvrier(ouvrier);
                    return this.RedirectToAction("Index", "Employes");
                }
                catch (BusinessException exception)
                {
                    this.ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return this.View("EditionOuvrier", viewModel);
        }
    }
}