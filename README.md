### DEV NOTES
Create tests first to ensure logic remains unchanged after refactor.
* Concrete DataStore means I cannot mock the response, decreasing test ability

Concrete DataStore from PaymentService could be removed adding an interface layer.
* Dupe stores, could be one?

There are methods for GetAccount and UpdateAccount but nothing for actioning the payment? (send to creditor)
* Going to assume this is out of scope for this tech test...
	* If it wasn't out of scope, the call would only happen if the scheme is valid before we update the account info.

PaymentScheme switch refactor to remove the long conditional.
* Factory?
	* Easy to extend

### IMPLEMENTATION
Create DataService to add a layer of abstraction between the PaymentService and AccountDataStore
* DataService will control DataStore for both primary and backup, removing need for potential dupe code.

Create Scheme factory to replace the switch section making the PaymentService cleaner and isolates the validation logic.
* Easier to add new schemas and update existing ones.

### FINAL NOTES
