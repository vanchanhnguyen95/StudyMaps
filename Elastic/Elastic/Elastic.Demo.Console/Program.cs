// See https://aka.ms/new-console-template for more information
using ElasticHelper;
using Nest;

Console.WriteLine("Hello, World!");

string INDEX_NAME = "newtestdb1";
string CATEGORY = "LCD TV";
string documentId = "101";

Product product = new Product
{
    id = documentId,
    name = "Samsung TV",
    category = "LCD TV",
    description = "Samsung LCD 100 inch TV",
    isPopularProduct = 1,
    price = 50000,
    keyWords = "Tv,LCD,SAMSUNG TV"
};

//1. Connect to Elastic Search.
ElasticClient elasticClient = ElasticsearchHelper.GetESClient();

//2. Creating Documents In Elasticsearch.
ElasticsearchHelper.CreateDocument(elasticClient, INDEX_NAME, product, documentId);

//3. Getting Documents From Elasticsearch by document id.
ElasticsearchHelper.GetDocument(elasticClient, INDEX_NAME, documentId);

//4. Get all products by Category
ElasticsearchHelper.GetProductByCategory(elasticClient, INDEX_NAME, CATEGORY);

//5. Get all products by price range.
ElasticsearchHelper.GetProductByPriceRange(elasticClient, INDEX_NAME, 5000, 60000);

//6. Get all products by Category and price range.
ElasticsearchHelper.GetProductByCategoryPriceRange(elasticClient, INDEX_NAME, CATEGORY, 5000, 60000);

//7. Updating Documents In Elasticsearch.
product.description = "Samsung SMART LCD TV";
ElasticsearchHelper.UpdateDocument(elasticClient, INDEX_NAME, product, documentId);
ElasticsearchHelper.GetDocument(elasticClient, INDEX_NAME, documentId);

//8.Deleting Documents From Elasticsearch.
ElasticsearchHelper.DeleteDocument(elasticClient, INDEX_NAME, documentId);
Console.ReadKey();