# Unreleased

## Features

- `QueryMethodCodingStyle` feature is now added which marks methods of a query
  as `QueryMethod` along with `sort`, `skip` and `take` parameters
-  New UX features are introduced in `Monolith` recipe
  - `QuerActionAsDataContainerUxFeature` to configure descriptor properties of
    query methods 

## Improvements

- `DataContainer` component is added to render enumerable datas with basic input
  support

## Bugfixes

- Component action having `undefined` model value when `Input` was configured 
  query bound, fixed