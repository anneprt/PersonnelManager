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
			var serviceEmploye = new ServiceEmploye(fauxDataEmploye.Object);
			Assert.;
		}

		[TestMethod]
		public void DateEmbaucheOuvrierAnterieureAujourdhuiPlus3Mois()
		{
			Assert.Fail();
		}

		[TestMethod]
		public void SalaireCadrePositif()
		{
			Assert.Fail();
		}

		[TestMethod]
		public void TauxHoraireOuvrierPositif()
		{
			Assert.Fail();
		}

		[TestMethod]
		public void InterdireCaracteresSpeciauxDansNomEtPrenomCadre()
		{
			Assert.Fail();
		}

		[TestMethod]
		public void InterdireCaracteresSpeciauxDansNomEtPrenomOuvrier()
		{
			Assert.Fail();
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
