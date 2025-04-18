---
pages:
  - application
  - layer
  - feature
---

# Architecture

Baked has an architectural core that is designed to allow us to build software
with any architectural style. Core idea is to enable a good separation of
concerns without causing any repetitive work. To achieve this Baked breaks down
an application into following components: domain objects, layers and features.

Let's dive into each type of software component to have a better understanding
of how Baked is structured.

## Domain Objects

At the core of an application lies its domain logic. By this we understand the
very reason for an application to exist. Everything else exists only to expose
this domain to outside world. So first component is domain objects;

```mermaid
flowchart
  D((Domain<br/>Objects))
```

## Layers

Second component is layers. Every layer introduces a new technology, such as a
database server, message queue server or a framework, into your application
architecture.

> [!TIP]
>
> Layers are named after the concept of their technology.

```mermaid
flowchart LR
  HS[[HTTP<br/>Server<br/>Layer]]
  subgraph DL[Domain Layer]
    D((Domain<br/>Objects))
  end
  DA[(Data<br/>Access<br/>Layer)]
  HS ~~~ DL ~~~ DA
```

In this example, `HttpServer` introduces the concept of web as a layer through
the `ASP.NET Core` technology. Also, `DataAccess` introduces the concept of
relational databases as a layer through the `NHibernate` framework.

> [!NOTE]
>
> `Domain` is a unique layer that introduces the business logic of an
> application by encapsulating its domain objects. It processes the assemblies
> to create an application schema, so that it can be used in creating
> configuration, conventions and/or generating source code.

For domain objects to be exposed as API endpoints or mapped onto a relational
database, layers require an insight about how to interpret domain objects.

This is why every layer comes with its own configuration that is specific to
the technology it uses. Configurations may contain a set of helper classes
and/or facade methods that makes it easy to build a certain type of
configuration, but they do __not__ have opinions upfront.

## Features

Finally, features connect layers in an opinionated way to introduce new
behaviours to the application. Features consist of two parts; an optional
abstraction (port) and a required implementation (adapter).

### Abstraction

An abstraction is the library that contains all common classes, interfaces,
attributes, if any, for a feature. It is the only accessible part of a feature
from domain objects or from other feature implementations.

> [!NOTE]
>
> Each feature has only one abstraction, named after the ability it provides,
> e.g. `Authentication`, `Lifetime`, `Logging`, `Orm` etc.

```mermaid
flowchart
  subgraph Layers
    HS[[HTTP<br/>Server<br/>Layer]]
    subgraph DL[Domain Layer]
      D((Domain<br/>Objects))
    end
    DA[(Data<br/>Access<br/>Layer)]
  end

  subgraph Features
    subgraph Abstraction
      A(Authentication)
      Db(Database)
    end
  end

  D -.uses.-> A
  D -.uses.-> Db
```

### Implementation

This is the implementation part of a feature that provides concrete
implementations for the interfaces defined in the abstraction, along with
opinionated configurations using the configuration API of the corresponding
layer(s).

Features may have multiple implementations, each named after its corresponding
design or technology, e.g. `Authentication.FixedBearerToken` and
`Database.MySql`.

> [!TIP]
>
> `Lifetime` represents lifetime feature that provides an API to your domain to
> register services in different lifetimes. `Lifetime.Singleton` and
> `Lifetime.Transient` are two different implementations of how you register and
> retrieve your domain objects from dependency injection.

In conclusion, feature implementations serve as bridges, connecting layers to
each other through their opinionated configurations. Below is a complete sample
architecture, showcasing the integration of domain logic, layers and features;

```mermaid
flowchart TB
  subgraph Layers
    HS[[HTTP<br/>Server<br/>Layer]]
    subgraph DL[Domain Layer]
      D((Domain<br/>Objects))
    end
    DA[(Data<br/>Access<br/>Layer)]
  end

  subgraph Features
    subgraph Abstraction
      A(Authentication)
      Db(Database)
    end

    subgraph Implementation
      AF(Authentication.FixedBearerToken)
      DbM(Database.MySql)
    end
  end

  HS -.configured by.-> AF
  A --implemented by--> AF
  D -.uses.-> A
  D -.uses.-> Db
  Db --implemented by--> DbM
  DA -.configured by.-> DbM
```

[Clean Architecture]: https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures#clean-architecture
