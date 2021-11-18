## 7/10 ##
### Item class ###
- [x] name of the object (string)
- [x] description of the object (string)
- [x] value of the item (float) [DID AS DECIMAL, WITH AUTHORISATION]

### Shop class ###
- [x] List of sellable items
- [x] Function that initialises all (at least 5) items with their data (name, value and description)
- [x] Function that presents the sellable items in the console for the player to buy from
- [x] Basket that holds items that the player chose but not yet bought
- [x] Function that adds the price of all items in basket and checks if the players has enough money
- [ ] if yes then the money is paid and items can be transferred to the player [IMPLEMENTING]
- [x] if no the player should be presented with the information (missing money and how much) and allowed to reset the basket to start over

### Player class ###
- [x] money that is used to buy items (float) [DID AS DECIMAL, WITH AUTHORISATION
- [ ] List of bought items [IMPLEMENTING: INVENTORY]
- [ ] Function to show stored items [IMPLEMENTING]

## 8/10 ##
- [x] the ability to do multiple buy runs with persistent player inventory states
- [x] if starting money is 100 and first buy used 20 then the second time the player will only have 80
- [ ] the ability to edit basket anytime by removing objects one by one [IMPLEMENTING]

## 9/10 ##
- [ ] ability to sell items back to the shop at a reduced rate [IMPLEMENTING]

## 10/10 ##
- [x] items are now limited and have a stack system [IMPLEMENTED -- In Progress]
- [x] if 0 stack it cannot be bought again (potentially remove from list) sold items need to be re-added to shop stack [IMPLEMENTING]
