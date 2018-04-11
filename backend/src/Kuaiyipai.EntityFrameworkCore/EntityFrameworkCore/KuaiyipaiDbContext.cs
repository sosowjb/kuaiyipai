﻿using Abp.IdentityServer4;
using Abp.Zero.EntityFrameworkCore;
using Kuaiyipai.Auction.Entities;
using Kuaiyipai.Authorization.Roles;
using Kuaiyipai.Authorization.Users;
using Kuaiyipai.Chat;
using Kuaiyipai.Editions;
using Kuaiyipai.Friendships;
using Kuaiyipai.MultiTenancy;
using Kuaiyipai.MultiTenancy.Accounting;
using Kuaiyipai.MultiTenancy.Payments;
using Kuaiyipai.Storage;
using Microsoft.EntityFrameworkCore;

namespace Kuaiyipai.EntityFrameworkCore
{
    public class KuaiyipaiDbContext : AbpZeroDbContext<Tenant, Role, User, KuaiyipaiDbContext>, IAbpPersistedGrantDbContext
    {
        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }


        public virtual DbSet<Address> Addresses { get; set; }

        public virtual DbSet<Area> Areas { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Pillar> Pillars { get; set; }

        public virtual DbSet<ItemDrafting> ItemsDrafting { get; set; }

        public virtual DbSet<ItemAuctioning> ItemsAuctioning { get; set; }

        public virtual DbSet<ItemCompleted> ItemsCompleted { get; set; }

        public virtual DbSet<ItemTerminated> ItemsTerminated { get; set; }

        public virtual DbSet<ItemPic> ItemPictures { get; set; }

        public virtual DbSet<OrderWaitingForPayment> OrdersWaitingForPayment { get; set; }

        public virtual DbSet<OrderWaitingForSending> OrdersWaitingForSending { get; set; }

        public virtual DbSet<OrderWaitingForReceiving> OrdersWaitingForReceiving { get; set; }

        public virtual DbSet<OrderWaitingForEvaluating> OrdersWaitingForEvaluating { get; set; }

        public virtual DbSet<OrderCompleted> OrdersCompleted { get; set; }

        public virtual DbSet<UserBalance> UserBalances { get; set; }

        public virtual DbSet<UserBalanceRecord> UserBalanceRecords { get; set; }

        public virtual DbSet<UserBiddingRecord> UserBidRecords { get; set; }


        public KuaiyipaiDbContext(DbContextOptions<KuaiyipaiDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BinaryObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { e.PaymentId, e.Gateway });
            });

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}
