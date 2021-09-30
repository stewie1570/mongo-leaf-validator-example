# mongo-leaf-validator-example

**Hosted at**: [https://stewie-mongo-test.herokuapp.com/](https://stewie-mongo-test.herokuapp.com/)

**Getting Started**
- Create a MongoDB with a **Contacts** collection
- Add one document to it:
```js
{
    "contactNumber": 0,
}
```
- Add **appsettings.Development.json** to the root directory.
- Add the key **MongoConnectionString** for persistence.
- Add the key **RedisConnectionString** for authentication.
