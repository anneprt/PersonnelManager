using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersonnelManager.Business.Exceptions;
using PersonnelManager.Business.Services;
using PersonnelManager.Dal.Data;
using PersonnelManager.Dal.Entites;

namespace PersonnelManager.Business.Tests
{
    [TestClass]
    public class ServiceEmployeTest
    {
        [TestMethod]
        public void ValiderNomEtPrenomRequis()
        {
            Assert.IsTrue(
                TestsHelper.HasAttribute<Employe, RequiredAttribute>(x => x.Nom));
            Assert.IsTrue(
                TestsHelper.HasAttribute<Employe, RequiredAttribute>(x => x.Prenom));
        }
        [TestMethod]
        public void DateEmbaucheOuvrierPosterieureA1920()
        {
            var fauxDataEmploye = new Mock<IDataEmploye>();

            fauxDataEmploye.Setup(x => x.EnregistrerOuvrier(It.IsAny<Ouvrier>()));

            var serviceEmploye = new ServiceEmploye(fauxDataEmploye.Object);
            var ouvrier = new Ouvrier
            {
                Nom = "Dupont",
                Prenom = "Gérard",
                DateEmbauche = new DateTime(1920, 12, 31),
                TauxHoraire = 12
            };
            var exception = Assert.ThrowsException<BusinessException>(() =>
            {
                serviceEmploye.EnregistrerOuvrier(ouvrier);
            });
            Assert.AreEqual("La date d'embauche doit être > 1920",
               exception.Message);
        }

        [TestMethod]
        public void DateEmbaucheCadreAnterieureAujourdhuiPlus3Mois()
        {
            var fauxDataEmploye = new Mock<IDataEmploye>();

            fauxDataEmploye.Setup(x => x.EnregistrerCadre(It.IsAny<Cadre>()));

            var serviceEmploye = new ServiceEmploye(fauxDataEmploye.Object);

            var cadre = new Cadre
            {
                Nom = "Boole",
                Prenom = "Bill",
                DateEmbauche = new DateTime(2020, 07, 30),
                SalaireMensuel = 2050
            };
            var exception = Assert.ThrowsException<BusinessException>(() =>
              {
                  serviceEmploye.EnregistrerCadre(cadre);
              });
            Assert.AreEqual("La date d'embauche doit être inférieure à 3 mois à partir d'aujourdhui", exception.Message);

        }

        [TestMethod]
        public void DateEmbaucheOuvrierAnterieureAujourdhuiPlus3Mois()
        {
            var fauxDataEmploye = new Mock<IDataEmploye>();

            fauxDataEmploye.Setup(x => x.EnregistrerOuvrier(It.IsAny<Ouvrier>()));

            var serviceEmploye = new ServiceEmploye(fauxDataEmploye.Object);

            var ouvrier = new Ouvrier
            {
                Nom = "Lucky",
                Prenom = "Luke",
                DateEmbauche = new DateTime(2020, 07, 30),
                TauxHoraire = 15
            };
            var exception = Assert.ThrowsException<BusinessException>(() =>
            {
                serviceEmploye.EnregistrerOuvrier(ouvrier);
            });
            Assert.AreEqual("La date d'embauche doit être inférieure à 3 mois à partir d'aujourdhui", exception.Message);

        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void SalaireCadrePositif(int salaire)
        {
            var fauxDataEmploye = new Mock<IDataEmploye>();

            fauxDataEmploye.Setup(x => x.EnregistrerCadre(It.IsAny<Cadre>()));

            var serviceEmploye = new ServiceEmploye(fauxDataEmploye.Object);

            var cadre = new Cadre
            {

                Nom = "Marty",
                Prenom = "McFly",
                DateEmbauche = DateTime.Today,
                SalaireMensuel = salaire
            };
            var exception = Assert.ThrowsException<BusinessException>(() =>
            {
                serviceEmploye.EnregistrerCadre(cadre);
                serviceEmploye.GetSalaireCadre(1, DateTime.Parse("01/08/2018"));
            });
            Assert.AreEqual("Salaire mensuel invalide", exception.Message);
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void TauxHoraireOuvrierPositif(int tauxHoraire)
        {
            var fauxDataEmploye = new Mock<IDataEmploye>();

            fauxDataEmploye.Setup(x => x.EnregistrerOuvrier(It.IsAny<Ouvrier>()));

            var serviceEmploye = new ServiceEmploye(fauxDataEmploye.Object);

            var ouvrier = new Ouvrier
            {
                Nom = "Doc",
                Prenom = "Brown",
                DateEmbauche =DateTime.Today,
                TauxHoraire = tauxHoraire
            };
            var exception = Assert.ThrowsException<BusinessException>(() =>
            {
                serviceEmploye.EnregistrerOuvrier(ouvrier);
                serviceEmploye.GetSalaireOuvrier(1, DateTime.Parse("01/08/2018"));
            });
            Assert.AreEqual("Taux horaire invalide", exception.Message);
        }

        [DataTestMethod]
        [DataRow("DUPONT", "Gérard", true)]
        [DataRow("DE-LATTREILLE", "Jean-Philippe", true)]
        [DataRow("DUPONT", "François", true)]
        [DataRow("DUPONT3", "Jean", false)]
        [DataRow("DUPONT", "F@bien", false)]
        public void InterdireCaracteresSpeciauxDansNomEtPrenomCadre(string nom, string prenom, bool estValide)
        {
            var fauxDataEmploye = new Mock<IDataEmploye>();
            var serviceEmploye = new ServiceEmploye(fauxDataEmploye.Object);

            var cadre = new Cadre
            {

                Nom = nom,
                Prenom = prenom,
                DateEmbauche = DateTime.Today,
                SalaireMensuel = 2050
            };

            if (!estValide)
            {
                var exception = Assert.ThrowsException<BusinessException>(
                    () => serviceEmploye.EnregistrerCadre(cadre));
                Assert.AreEqual("Entrée invalide caractères spéciaux interdits",
                    exception.Message);
            }
            else
            {
                serviceEmploye.EnregistrerCadre(cadre);
            }

        }

        [DataTestMethod]
        [DataRow("DUPONT", "Gérard", true)]
        [DataRow("DE-LATTREILLE", "Jean-Philippe", true)]
        [DataRow("DUPONT", "François", true)]
        [DataRow("DUPONT3", "Jean", false)]
        [DataRow("DUPONT", "F@bien", false)]
        public void InterdireCaracteresSpeciauxDansNomEtPrenomOuvrier(string nom, string prenom, bool estValide)
        {
            var fauxDataEmploye = new Mock<IDataEmploye>();
            var serviceEmploye = new ServiceEmploye(fauxDataEmploye.Object);

            var ouvrier = new Ouvrier
            {

                Nom = nom,
                Prenom = prenom,
                DateEmbauche = DateTime.Today,
                TauxHoraire = 15

            };

            if (!estValide)
            {
                var exception = Assert.ThrowsException<BusinessException>(
                    () => serviceEmploye.EnregistrerOuvrier(ouvrier));
                Assert.AreEqual("Entrée invalide caractères spéciaux interdits",
                    exception.Message);
            }
            else
            {
                serviceEmploye.EnregistrerOuvrier(ouvrier);
            }
        }

        [TestMethod]
        public void OuvrierEstNonNull()
        {
            var fauxDataEmploye = new Mock<IDataEmploye>();
            var serviceEmploye = new ServiceEmploye(fauxDataEmploye.Object);
            Assert.ThrowsException<InvalidOperationException>(
                () => serviceEmploye.EnregistrerOuvrier(null));
        }

        [TestMethod]
        public void CadreEstNonNull()
        {
            var fauxDataEmploye = new Mock<IDataEmploye>();
            var serviceEmploye = new ServiceEmploye(fauxDataEmploye.Object);
            Assert.ThrowsException<InvalidOperationException>(
                () => serviceEmploye.EnregistrerCadre(null));
        }
    }
}
