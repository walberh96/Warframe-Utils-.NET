# Warframe Market API v2 Documentation

## Overview

This document provides comprehensive information about the Warframe Market API v2 that is used in this project.

**Source:** [Official WFM API v2 Documentation](https://42bytes.notion.site/WFM-Api-v2-Documentation-5d987e4aa2f74b55a80db1a09932459d)

**API Version:** v0.21.2

## General Information

### Base URLs

- **API Base Path:** `https://api.warframe.market/v2/`
- **Static Assets:** `https://warframe.market/static/assets/`
  - Used for item icons, user avatars, and profile backgrounds
  - Example path: `items/images/en/dual_rounds.304395bed5a40d76ddb9a62c76736d94.png`

### Rate Limits

- **3 requests per second**
- Exceeding rate limits results in a **429 error code**
- Excessive RPS may trigger CloudFlare blocking
- Too many concurrent connections return a **509 error code**

### Global Headers

#### Language Header

Support for 12 languages: `ko`, `ru`, `de`, `fr`, `pt`, `zh-hans`, `zh-hant`, `es`, `it`, `pl`, `uk`, `en`

```http
Language: {lang}
```

**Default:** `en`

For endpoints with i18n fields, this header requests translations in addition to English.

**Example Response:**
```json
{
  "i18n": {
    "en": {...},
    "ko": {...}
  }
}
```

#### Platform Header

Supported platforms: `pc`, `ps4`, `xbox`, `switch`, `mobile`

```http
Platform: {platform}
```

**Default:** `pc`

#### Crossplay Header

Controls cross-platform trading visibility.

```http
Crossplay: true|false
```

**Default:** `true`

- `true` - Include cross-play orders from other platforms
- `false` - Exclude cross-play orders
- Note: You'll still receive orders from players on your platform who have crossplay enabled

### Response Structure

All API responses follow this structure:

```json
{
  "apiVersion": "x.x.x",
  "data": payload | null,
  "error": payload | null
}
```

**Fields:**
- `apiVersion` - Semantic version of the API server
- `data` - Response data if successful, otherwise null
- `error` - Error information if failed, otherwise null

### Error Handling

#### Handled Errors

```json
{
  "apiVersion": "x.x.x",
  "data": null,
  "error": {
    "request": [...],
    "inputs": {
      "fieldOne": "",
      "fieldTwo": "",
      ...
    }
  }
}
```

**Error Fields:**
- `request` - General request errors (e.g., `app.errors.unauthorized`, `app.errors.forbidden`)
- `inputs` - Field-level validation errors (e.g., `{platinum: "app.field.tooSmall"}`)

#### Unhandled Errors

- Returns **5xx** status code without content
- Indicates critical server error

#### Rate Limit Errors

- **429** - Rate limit exceeded (CloudFlare challenge page)
- **509** - Too many concurrent connections from same IP

## Endpoint Markers

- 🔒 **Requires authorization** - Must include valid access token
- 💔 **First party apps only** - WFM Frontend, iOS, Android apps
- 🌀 **Platform/Crossplay dependent** - Content varies by platform and crossplay headers
- 🇬🇧 **Supports i18n** - Can request additional translations
- 🚧 **Unstable** - Use at your own risk
- 🚫 **Deprecated** - Will be removed in future versions

## Endpoints

### Manifests

#### GET /v2/versions

Retrieves current version numbers for resources and mobile apps.

**Response:**
```json
{
  "apiVersion": "x.x.x",
  "data": {
    "apps": {
      "ios": "x.x.x",
      "android": "x.x.x",
      "minIos": "x.x.x",
      "minAndroid": "x.x.x"
    },
    "collections": {
      "items": "base64",
      "rivens": "base64",
      "liches": "base64",
      "sisters": "base64",
      "missions": "base64",
      "npcs": "base64",
      "locations": "base64"
    },
    "updatedAt": "2021-05-21T14:59:02Z"
  },
  "error": null
}
```

#### GET /v2/items 🇬🇧

Get list of all tradable items.

**Returns:** Array of `ItemShort` models

#### GET /v2/item/{slug} 🇬🇧

Get full information about a specific item.

**URL Parameters:**
- `slug` - Item identifier from Item/ItemShort models

**Returns:** `Item` model

#### GET /v2/item/{slug}/set 🇬🇧

Retrieve information on item sets.

**URL Parameters:**
- `slug` - Item identifier

**Returns:**
```json
{
  "id": "54a73e65e779893a797fff72",
  "items": [Item, Item, ...]
}
```

**Behavior:**
- If item is standalone → returns array with single item
- If item is part of set → returns all items in that set

#### GET /v2/riven/weapons 🇬🇧

Get list of all tradable riven items.

**Returns:** Array of `Riven` models

#### GET /v2/riven/weapon/{slug} 🇬🇧

Get full information about a specific riven weapon.

**URL Parameters:**
- `slug` - Riven identifier

**Returns:** `Riven` model

#### GET /v2/riven/attributes 🇬🇧

Get list of all riven weapon attributes.

**Returns:** Array of `RivenAttribute` models

#### GET /v2/lich/weapons 🇬🇧

Get list of all tradable lich weapons.

**Returns:** Array of `LichWeapon` models

#### GET /v2/lich/weapon/{slug} 🇬🇧

Get full information about a specific lich weapon.

**Returns:** `LichWeapon` model

#### GET /v2/lich/ephemeras 🇬🇧

Get list of all tradable lich ephemeras.

**Returns:** Array of `LichEphemera` models

#### GET /v2/lich/quirks 🇬🇧

Get list of all tradable lich quirks.

**Returns:** Array of `LichQuirk` models

#### GET /v2/sister/weapons 🇬🇧

Get list of all tradable sister weapons.

**Returns:** Array of `SisterWeapon` models

#### GET /v2/sister/weapon/{slug} 🇬🇧

Get full information about a specific sister weapon.

**Returns:** `SisterWeapon` model

#### GET /v2/sister/ephemeras 🇬🇧

Get list of all tradable sister ephemeras.

**Returns:** Array of `SisterEphemera` models

#### GET /v2/sister/quirks 🇬🇧

Get list of all tradable sister quirks.

**Returns:** Array of `SisterQuirk` models

#### GET /v2/locations 🇬🇧

Get list of all locations known to WFM.

**Returns:** Array of `Location` models

#### GET /v2/npcs 🇬🇧

Get list of all NPCs known to WFM.

**Returns:** Array of `Npc` models

#### GET /v2/missions 🇬🇧

Get list of all Missions known to WFM.

**Returns:** Array of `Mission` models

### Orders

#### GET /v2/orders/recent 🌀

Get the most recent orders (last 4 hours, max 500, sorted by createdAt).

**Cache:** 1 minute refresh interval

**Returns:** Array of `OrderWithUser` models

#### GET /v2/orders/item/{slug} 🌀

Get all orders for an item from users online within the last 7 days.

**URL Parameters:**
- `slug` - Item identifier

**Returns:** Array of `OrderWithUser` models

#### GET /v2/orders/item/{slug}/top 🌀

Get top 5 buy and top 5 sell orders for an item from online users only. Orders sorted by price.

**URL Parameters:**
- `slug` - Item identifier

**Query Parameters:**
- `rank` (int) - Filter by exact rank (0 to max rank)
- `rankLt` (int) - Filter by rank less than value (1 to max rank)
- `charges` (int) - Filter by exact charges left (0 to max charges)
- `chargesLt` (int) - Filter by charges less than value (1 to max charges)
- `amberStars` (int) - Filter by exact amber stars (0 to max)
- `amberStarsLt` (int) - Filter by amber stars less than value (1 to max)
- `cyanStars` (int) - Filter by exact cyan stars (0 to max)
- `cyanStarsLt` (int) - Filter by cyan stars less than value (1 to max)
- `subtype` (string) - Filter by item subtype (e.g., "crafted", "blueprint")

**Response:**
```json
{
  "data": {
    "buy": [OrderWithUser, ...],
    "sell": [OrderWithUser, ...]
  }
}
```

#### GET /v2/orders/user/{slug}
#### GET /v2/orders/userId/{userId}

Get public orders from a specific user.

**URL Parameters:**
- `slug` - User slug from User/UserShort models
- `userId` - User ID from User/UserShort models

**Returns:** Array of `Order` models

#### GET /v2/orders/my 🔒

Retrieve all orders for the currently authenticated user.

**Returns:** Array of `Order` models

#### GET /v2/order/{id}

Get specific order by ID.

**URL Parameters:**
- `id` - Order ID

**Returns:** `OrderWithUser` model

#### POST /v2/order 🔒

Create a new order.

**Request Body:**
```json
{
  "itemId": "54aae292e7798909064f1575",
  "type": "sell",
  "platinum": 38,
  "quantity": 12,
  "visible": true,
  "perTrade": 6,
  "rank": 5,
  "charges": 3,
  "subtype": "blueprint",
  "amberStars": 3,
  "cyanStars": 3
}
```

**Request Fields:**
- `itemId` (string, required) - Item ID
- `type` (string, required) - "sell" or "buy"
- `platinum` (int, required) - Price
- `quantity` (int, required) - Stock amount
- `visible` (bool, required) - Order visibility
- `perTrade` (int) - Minimum items per transaction
- `rank` (int) - Item rank (for mods)
- `charges` (int) - Remaining charges (for parazon mods)
- `subtype` (string) - Item subtype
- `amberStars` (int) - Installed amber stars
- `cyanStars` (int) - Installed cyan stars

**Returns:** `Order` model

#### PATCH /v2/order/{id} 🔒

Update an existing order.

**URL Parameters:**
- `id` - Order ID

**Request Body:**
```json
{
  "platinum": 10,
  "quantity": 12,
  "perTrade": 3,
  "rank": 1,
  "charges": 2,
  "amberStars": 2,
  "cyanStars": 2,
  "subtype": "blueprint",
  "visible": false
}
```

**Returns:** `Order` model

#### DELETE /v2/order/{id} 🔒

Delete an order.

**URL Parameters:**
- `id` - Order ID

**Returns:** `Order` model

#### POST /v2/order/{id}/close 🔒

Close a portion or all of an existing order.

**URL Parameters:**
- `id` - Order ID

**Request Body:**
```json
{
  "quantity": 12
}
```

**Example:** If order has 20 units and you close 8, remaining will be 12. Closing all units removes the order.

**Returns:** `Transaction` model (without User model)

#### PATCH /v2/orders/group/{id} 🔒

Update a group of orders.

**URL Parameters:**
- `id` - Group ID (currently: "all" or "ungrouped")

**Request Body:**
```json
{
  "visible": false,
  "type": "sell"
}
```

**Request Fields:**
- `visible` (bool) - Set visibility for all orders in group
- `type` (string) - Target only "sell" or "buy" orders

**Response:**
```json
{
  "data": {
    "updated": 13
  }
}
```

### Users

#### GET /v2/me 🔒

Get information about the currently authenticated user.

**Returns:** `UserPrivate` model

#### PATCH /v2/me 🔒

Update your user profile.

**Request Body:**
```json
{
  "about": "something",
  "platform": "mobile",
  "crossplay": true,
  "locale": "pt",
  "theme": "light",
  "syncLocale": true,
  "syncTheme": true
}
```

**Request Fields:**
- `about` (string) - Profile description
- `platform` (string) - Main platform (pc, ps4, xbox, switch, mobile)
- `crossplay` (bool) - Crossplay enabled status
- `locale` (string) - Preferred language
- `theme` (string) - UI theme preference
- `syncLocale` (bool) - Sync locale across devices
- `syncTheme` (bool) - Sync theme across devices

**Returns:** `UserPrivate` model

#### POST /v2/me/avatar 🔒

Update your avatar.

**Content-Type:** `multipart/form-data`

**Form Data:**
- `avatar` (file) - Image file

**Constraints:**
- Formats: .png, .jpg, .jpeg, .webp, .gif, .bmp, .avif
- Max size: 5MB
- Resized to 256×256 pixels (centered)

**Returns:** `UserPrivate` model

#### POST /v2/me/background 🔒

Update your profile background.

**Requirements:** Silver subscription tier or higher

**Content-Type:** `multipart/form-data`

**Form Data:**
- `background` (file) - Image file

**Constraints:**
- Formats: .png, .jpg, .jpeg, .webp, .gif, .bmp, .avif
- Max size: 8MB
- Resized to 1920×820 pixels (centered)

**Returns:** `UserPrivate` model

#### GET /v2/user/{slug}
#### GET /v2/userId/{userId}

Get information about a specific user.

**URL Parameters:**
- `slug` - User slug
- `userId` - User ID

**Returns:** `User` model

### Achievements

#### GET /v2/achievements

Get list of all available achievements (excluding secret ones).

**Returns:** Array of `Achievement` models (without state)

#### GET /v2/achievements/user/{slug}
#### GET /v2/achievements/userId/{userId}

Get all achievements for a specific user.

**URL Parameters:**
- `slug` - User slug
- `userId` - User ID

**Query Parameters:**
- `featured` (bool) - Return only featured achievements

**Returns:** Array of `Achievement` models (with state)

### Authentication

#### POST /auth/signin 💔

User login (first party apps only).

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "password123",
  "clientId": "wfm-0000",
  "deviceId": "unique-device-id",
  "deviceName": "My Device"
}
```

**Headers:**
- `X-Firebase-AppCheck` (string) - AppCheck verification token

**Response:**
```json
{
  "data": {
    "accessToken": "...",
    "refreshToken": "...",
    "tokenType": "Bearer",
    "expiresIn": 12345
  }
}
```

#### POST /auth/signup 💔

User registration (first party apps only).

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "password123",
  "clientId": "wfm-0000",
  "deviceId": "unique-device-id",
  "deviceName": "My Device",
  "platform": "pc",
  "locale": "en"
}
```

**Headers:**
- `X-Firebase-AppCheck` (string) - AppCheck verification token

**Response:**
```json
{
  "data": {
    "accessToken": "...",
    "refreshToken": "...",
    "tokenType": "Bearer",
    "expiresIn": 12345
  }
}
```

#### POST /auth/refresh 🔒

Refresh session tokens.

**Request Body:**
```json
{
  "grantType": "refresh_token",
  "clientId": "wfm-0000",
  "deviceId": "device-id",
  "refreshToken": "JwtRefreshToken"
}
```

**Headers (First party apps only):**
- `X-Firebase-AppCheck` (string) - AppCheck verification token

**Response:**
```json
{
  "data": {
    "accessToken": "...",
    "refreshToken": "...",
    "tokenType": "Bearer",
    "expiresIn": 12345
  }
}
```

#### POST /auth/signout 🔒

Terminate current session.

**Headers:**
- `Authorization` - Bearer token

**Response:** Empty (200 status)

### Dashboard

#### GET /v2/dashboard/showcase 🇬🇧

Get mobile app main screen dashboard with featured items.

**Returns:** `DashboardShowcase` model

### OAuth 2.0

For third-party applications, OAuth 2.0 is available:

- `/oauth/authorize` - Authorize request
- `/oauth/token` - Exchange code for access token (with PKCE)
- `/oauth/revoke` - Revoke access token

**Note:** Authorization tokens from v1 API have default scope "all". The v1 login endpoint will eventually be removed in favor of OAuth 2.0.

## Implementation Notes for This Project

1. **Service Integration:**
   - The `WarframeMarketApiService.cs` implements calls to this API
   - Current implementation uses v2 endpoints for item and order data

2. **Rate Limiting:**
   - Implement appropriate throttling to respect 3 req/sec limit
   - Consider caching responses where appropriate

3. **Error Handling:**
   - Handle 429 (rate limit) and 509 (connection limit) errors gracefully
   - Implement retry logic with exponential backoff

4. **Headers:**
   - Always set appropriate Platform header (default: "pc")
   - Set Language header for localization if needed
   - Set Crossplay header based on user preferences

5. **Authentication:**
   - Store access and refresh tokens securely
   - Implement token refresh logic before expiration
   - Handle authentication errors gracefully

6. **Best Practices:**
   - Cache manifest data (items, rivens, etc.) and refresh based on version endpoint
   - Use the /top endpoint for displaying best prices instead of fetching all orders
   - Respect rate limits and implement proper error handling

## Related Documentation

- [Data Models](https://42bytes.notion.site/Data-Models-65e9ab01868c4dcca6ba499e68a04ac9)
- [Websockets](https://42bytes.notion.site/Websockets-1d8515beb0be806a87e9e0fc71aad9aa)
- [OAuth 2.0](https://42bytes.notion.site/OAuth-2-0-04e1e72398db4cf8ae1b7b1bae4abcc1)

## Version History

- **Current Version:** v0.21.2
- Last Updated: January 13, 2026

---

*This documentation is based on the official Warframe Market API documentation and is subject to change. Always refer to the official documentation for the most up-to-date information.*
