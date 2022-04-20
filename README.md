# WarehousePost
Warehouse database management through REST API.

__________________________________________________________________________________________________________________


# How it works

  Application is created for a company dealing with warehouse management and products that are in it.
  The database I use is presented below.

<p align="center">
  <img src=https://user-images.githubusercontent.com/74014874/164337508-efd8fe12-57c5-4725-81b9-ec3b5a9e9fe7.png
   >
</p>

__________________________________________________________________________________________________________________

  API allows you to add a new order to the database by responding to the 
<p align="center">
  <b>HTTP POST request to /api/warehouses</b>
</p>
  with the data of the following form:<br />  
  
<p align="center">
  <img src=https://user-images.githubusercontent.com/74014874/164339152-916ccafc-9eb3-4833-915e-575bd7a1b9fc.png
   >
</p>

  All fields are required and the amount must be greater than 0
  
__________________________________________________________________________________________________________________
  
  ## API meets the following requirements:
  1. Checks if the product and warehouse with the given id's exist.
  2. Checks if there is an order product purchase.
  3. Checks if the order has not been fulfilled.
  4. Returns error messages for individual situations.


  **API is connected to my database by default and to set up yours you need to change ConnectionString in the file appsettings.json**

__________________________________________________________________________________________________________________


