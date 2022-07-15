using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using Microsoft.Rest;
using System.IO;
using System;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Storage;
using Microsoft.Azure.Management.Network;

namespace AzureTemplate
{
    class Program
    {
        static void Main(string[] args)
        {
            var groupName = "CommerceLink-AppSource";
            var subscriptionId = "d101f937-6d8b-44b2-aae8-add6708417c4";
            var clientId = "cea731e7-2713-4c5f-a065-0ac15e3633bf";
            var clientSecret = "jvaLTmzodPjsC9Rvbh2Dq3vjGNQlkLDT8qBOpQR7JPA=";
            var tenantId = "75668f36-65d3-4d90-a990-4bb8ec16728f";
            var deploymentName = "CommerceLink-VM";


            //Console.WriteLine("Enter Resource Group Name");
            //groupName = Console.ReadLine();

            //Console.WriteLine("Enter Subscription Id");
            //subscriptionId = Console.ReadLine();

            //Console.WriteLine("Enter Client Id");
            //clientId = Console.ReadLine();

            //Console.WriteLine("Enter Client Secret");
            //clientSecret = Console.ReadLine();

            //Console.WriteLine("Enter Tenant Id");
            //tenantId = Console.ReadLine();

            //Console.WriteLine("Enter Deployment Name of your VM");
            //deploymentName = Console.ReadLine();


            var token = GetAccessTokenAsync(clientId, clientSecret, tenantId);
            var credential = new TokenCredentials(token.Result.AccessToken);

            var dpResult = CreateTemplateDeploymentAsync(credential,groupName,deploymentName,subscriptionId);
            Console.WriteLine(dpResult.Result.Properties.ProvisioningState);
            Console.ReadLine();

            //DeleteResourceGroupAsync(credential,groupName,subscriptionId);
            //Console.ReadLine();

        }

        private static async Task<AuthenticationResult> GetAccessTokenAsync(string clientId, string clientSecret, string tenantId)
        {
            var cc = new ClientCredential(clientId, clientSecret);
            var context = new AuthenticationContext("https://login.microsoftonline.com/" + tenantId);
            var token = await context.AcquireTokenAsync("https://management.azure.com/", cc);
            if (token == null)
            {
                throw new InvalidOperationException("Could not get the token.");
            }
            return token;
        }


        public static async Task<DeploymentExtended> CreateTemplateDeploymentAsync(TokenCredentials credential,
                                                                                   string groupName,
                                                                                   string deploymentName,
                                                                                   string subscriptionId)
        {
            Console.WriteLine("Creating the template deployment...");
            var deployment = new Deployment();
            deployment.Properties = new DeploymentProperties
            {
                Mode = DeploymentMode.Incremental,
                Template = File.ReadAllText("..\\..\\VirtualMachineTemplate.json"),
                Parameters = File.ReadAllText("..\\..\\Parameters.json")
            };
            var resourceManagementClient = new ResourceManagementClient(credential)
            {
                SubscriptionId = subscriptionId
            };

            return await resourceManagementClient.Deployments.CreateOrUpdateAsync(groupName,deploymentName,deployment);
        }

        public static async void DeleteResourceGroupAsync(TokenCredentials credential,string groupName,string subscriptionId)
        {
            Console.WriteLine("Deleting resource group...");
            var resourceManagementClient = new ResourceManagementClient(credential)
            {
                SubscriptionId = subscriptionId
            };
            await resourceManagementClient.ResourceGroups.DeleteAsync(groupName);
        }
    }
}
