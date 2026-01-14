-- Check existing users
SELECT "Id", "UserName", "Email", "EmailConfirmed", "LockoutEnd" 
FROM "AspNetUsers";

-- To manually confirm a user's email (if needed), uncomment and modify:
-- UPDATE "AspNetUsers" 
-- SET "EmailConfirmed" = true 
-- WHERE "Email" = 'your-email@example.com';
