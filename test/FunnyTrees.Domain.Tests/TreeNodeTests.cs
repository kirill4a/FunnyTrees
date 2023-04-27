using FluentAssertions;
using FunnyTrees.Common.Exceptions;
using FunnyTrees.Domain.Aggregates;

namespace FunnyTrees.Domain.Tests;

public class TreeNodeTests
{
    [Fact]
    public void TestCreateNodeFailName()
    {
        FluentActions.Invoking(() => new TreeNode(0, default, default))
            .Should().ThrowExactly<ArgumentNullException>();

        FluentActions.Invoking(() => new TreeNode(0, default, string.Empty))
            .Should().ThrowExactly<ArgumentNullException>();

        FluentActions.Invoking(() => new TreeNode(0, default, "    "))
            .Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void TestCreateNodeFailParent()
    {
        var name = "I\'m node";

        FluentActions.Invoking(() => new TreeNode(10, default, name))
            .Should().NotThrow();

        FluentActions.Invoking(() => new TreeNode(10, 11, name))
            .Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public void TestCreateNodeRoot()
    {
        var rootId = 5;
        var rootName = "I\'m the parent";

        var rootNode = new TreeNode(rootId, default, rootName);

        rootNode.Should().NotBeNull();
        rootNode.Id.Should().Be(rootId);
        rootNode.Name.Should().Be(rootName);
        rootNode.ParentId.Should().BeNull();
        rootNode.IsRoot.Should().BeTrue();
    }

    [Fact]
    public void TestCreateNodeWithParent()
    {
        var rootId = 5;
        var childId = 10;
        var rootName = "I\'m the parent";
        var childName = "I\'m your child node";

        var rootNode = new TreeNode(rootId, default, rootName);
        var chidlNode = new TreeNode(childId, rootId, childName);

        rootNode.Should().NotBeNull();
        rootNode.Id.Should().Be(rootId);
        rootNode.Name.Should().Be(rootName);
        rootNode.ParentId.Should().BeNull();
        rootNode.IsRoot.Should().BeTrue();

        chidlNode.Should().NotBeNull();
        chidlNode.Id.Should().Be(childId);
        chidlNode.Name.Should().Be(childName);
        chidlNode.ParentId.Should().NotBeNull().And.Be(rootNode.Id);
        chidlNode.IsRoot.Should().BeFalse();
    }

    [Fact]
    public void TestAddChildFail()
    {
        var fakeRootId = 9;
        var rootId = 10;
        var rootName = "I\'m the parent";

        var childId = 11;
        var childName = "I\'m the child";

        var rootNode = new TreeNode(rootId, default, rootName);
        var childNode = new TreeNode(childId, fakeRootId, childName);

        rootNode.Should().NotBeNull();
        childNode.Should().NotBeNull();
        rootNode.Id.Should().Be(rootId);
        childNode.Id.Should().Be(childId);
        rootNode.Children.Should().BeEmpty();
        childNode.ParentId.Should().Be(fakeRootId).And.NotBe(rootNode.Id);

        FluentActions.Invoking(() => rootNode.TryAddChild(childNode)).Should().ThrowExactly<SecureException>();
    }

    [Fact]
    public void TestAddChildOk()
    {
        var rootId = 1;
        var rootName = "I\'m the parent";

        var childId = 2;
        var childName = "I\'m the child";

        var rootNode = new TreeNode(rootId, default, rootName);
        var childNode = new TreeNode(childId, rootId, childName);

        rootNode.Should().NotBeNull();
        childNode.Should().NotBeNull();
        rootNode.Id.Should().Be(rootId);
        childNode.Id.Should().Be(childId);
        rootNode.Children.Should().BeEmpty();
        childNode.ParentId.Should().Be(rootNode.Id);

        rootNode.TryAddChild(childNode).Should().BeTrue();
        rootNode.Children.Should().NotBeEmpty();
        rootNode.Children.First().Should().Be(childNode);
    }
}