using System;
using System.Collections.Generic;

class Produit
{
    // Propriétés
    public string Nom { get; set; }
    public decimal Prix { get; set; }
    public int QuantiteEnStock { get; set; }

    //constructeur
    public Produit(string nom, decimal prix, int quantiteEnStock)
    {
        Nom = nom;
        Prix = prix;
        QuantiteEnStock = quantiteEnStock;
    }

}

class Panier
{
    private List<ArticlePanier> articles = new List<ArticlePanier>();

    public void AjouterProduit(Produit produit, int quantite)
    {
        if (produit.QuantiteEnStock >= quantite)
        {
            var article = articles.Find(a => a.Produit == produit);

            if (article != null)
            {
                // Le produit est déjà dans le panier : augmenter la quantité
                article.Quantite += quantite;
            }
            else
            {
                // Ajouter un nouvel article au panier
                articles.Add(new ArticlePanier(produit, quantite));
            }

            produit.QuantiteEnStock -= quantite;
            Console.WriteLine("Produit ajouté au panier");
        }
        else
        {
            Console.WriteLine("Stock insuffisant");
        }
    }

    public void RetirerProduit(Produit produit)
    {
        var article = articles.Find(a => a.Produit == produit);
        if (article != null)
        {
            produit.QuantiteEnStock += article.Quantite;
            articles.Remove(article);
            Console.WriteLine("Produit retiré du panier");
        }
        else
        {
            Console.WriteLine("Produit non trouvé dans le panier");
        }
    }

    public void AfficherTotal()
    {
        decimal total = 0;
        foreach (ArticlePanier article in articles)
        {
            total += article.SousTotal;
        }
        Console.WriteLine($"Total: {total:C}");
    }

    public void AfficherPanier()
    {
        if (articles.Count == 0)
        {
            Console.WriteLine("Votre panier est vide.");
            return;
        }

        Console.WriteLine("Contenu du panier :");
        foreach (ArticlePanier article in articles)
        {
            Console.WriteLine($"- {article.Produit.Nom} x {article.Quantite} @ {article.Produit.Prix:C} chacun (Sous-total: {article.SousTotal:C})");
        }
    }
}
class ArticlePanier
{
    //Propriétés
    public Produit Produit { get; set; }
    public int Quantite { get; set; }

    public ArticlePanier(Produit produit, int quantite)
    {
        Produit = produit;
        Quantite = quantite;
    }

    public decimal SousTotal => Produit.Prix * Quantite;
}

    class Program
{
    static void Main(string[] args)
    {
        // Création de quelques produits
        var produit1 = new Produit("Pomme", 0.5m, 100);
        var produit2 = new Produit("Banane", 0.3m, 50);
        var produit3 = new Produit("Orange", 0.7m, 75);

        // Création du panier
        var panier = new Panier();

        // Simuler une session d'achat
        Console.WriteLine("Début de la session d'achat en ligne.\n");
        string input = "d";
        while (input != "q")
        {
            Console.WriteLine("Produits disponibles:");
            Console.WriteLine($"1. {produit1.Nom} @ {produit1.Prix:C} chacun ({produit1.QuantiteEnStock} en stock)");
            Console.WriteLine($"2. {produit2.Nom} @ {produit2.Prix:C} chacun ({produit2.QuantiteEnStock} en stock)");
            Console.WriteLine($"3. {produit3.Nom} @ {produit3.Prix:C} chacun ({produit3.QuantiteEnStock} en stock)");
            Console.WriteLine("p. Afficher le panier");
            Console.WriteLine("s. Supprimer un article du panier");
            Console.WriteLine("q. Quitter");

            Console.Write("Sélectionnez un produit (1-3) ou p pour afficher le panier, s pour suprimer un article du pannier q pour quitter: ");
            input = Console.ReadLine();

            if (input == "q")
            {
                break;
            }

            else if (input == "p")
            {
                panier.AfficherPanier();
                panier.AfficherTotal();
                continue;
            }

            else if (input == "s")
            {
                Console.Write("Sélectionnez un produit à retirer du panier (1-3): ");
                input = Console.ReadLine();
                Produit produitToRemove = null;
                switch (input)
                {
                    case "1":
                        produitToRemove = produit1;
                        break;
                    case "2":
                        produitToRemove = produit2;
                        break;
                    case "3":
                        produitToRemove = produit3;
                        break;
                    default:
                        Console.WriteLine("Sélection invalide");
                        continue;
                }

                panier.RetirerProduit(produitToRemove);
                continue;
            }

            Produit produit = null;
            int quantite = 0;
            switch (input)
            {
                case "1":
                    produit = produit1;
                    break;
                case "2":
                    produit = produit2;
                    break;
                case "3":
                    produit = produit3;
                    break;
                default:
                    Console.WriteLine("Sélection invalide");
                    continue;
            }

            Console.Write("Quantité: ");
            if (!int.TryParse(Console.ReadLine(), out quantite))
            {
                Console.WriteLine("Quantité invalide");
                continue;
            }

            panier.AjouterProduit(produit, quantite);
        }

        Console.WriteLine("\nSession d'achat terminée.");
    }
}