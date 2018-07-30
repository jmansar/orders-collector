alter table OpenIdAccounts
add ProviderUri text not null constraint DF_OpenIdAccounts_ProviderUri default('');