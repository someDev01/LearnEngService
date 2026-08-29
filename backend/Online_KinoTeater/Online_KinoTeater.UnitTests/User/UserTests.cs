using Domain.Model.Entyties;

namespace Online_KinoTeater.UnitTests.User;

public class UserTests
{
    [Fact]
    public void Create_Should_CreateUser_When_EmailAndRoleIsValid()
    {
        // Arrange
        var email = Domain.Model.ValueObjects.Email.Create("example@mail.ru").Value!;
        const Role role = Role.User;
        
        // Act
        var user = Domain.Model.Entyties.User.Create(email, role);
        
        // Assert
        Assert.True(user.IsSuccess);
        Assert.NotNull(user.Value);
    }

    [Fact]
    public void Create_Should_ReturnFailure_When_EmailAndRoleIsNull()
    {
        // Act
        var user = Domain.Model.Entyties.User.Create(null, null);
        
        // Assert
        Assert.False(user.IsSuccess);
        Assert.Null(user.Value);
    }

    [Fact]
    public void Create_Should_HaveNullAvatarPath_When_EmailAndRoleIsValid()
    {
        // Arrange
        var email = Domain.Model.ValueObjects.Email.Create("example@mail.ru").Value!;
        
        // Act
        var user = Domain.Model.Entyties.User.Create(email, Role.User);
        
        // Assert
        Assert.True(user.IsSuccess);
        Assert.NotNull(user.Value);
        Assert.Null(user.Value.AvatarPath);
    }

    [Fact]
    public void UploadAvatar_Should_UpdateAvatarPath()
    {
        // Arrange
        var email = Domain.Model.ValueObjects.Email.Create("example@mail.ru").Value!;
        var user = Domain.Model.Entyties.User.Create(email, Role.User).Value!;
        const string avatarInput = "avatar/123.png";
        
        // Act 
        user.UploadAvatar(avatarInput);
        
        // Assert
        Assert.Equal(avatarInput, user.AvatarPath);
    }
    
}