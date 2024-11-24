using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Keyfactor.Extensions.Orchestrator.FortiWeb.Models.Responses
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Results
    {
        public int can_view { get; set; }
        public int q_ref { get; set; }
        public int can_clone { get; set; }
        public int q_type { get; set; }
        public string name { get; set; }

        [JsonProperty("deployment-mode")]
        public string deploymentmode { get; set; }

        [JsonProperty("deployment-mode_val")]
        public string deploymentmode_val { get; set; }
        public string protocol { get; set; }
        public string protocol_val { get; set; }
        public string ssl { get; set; }
        public string ssl_val { get; set; }
        public string implicit_ssl { get; set; }
        public string implicit_ssl_val { get; set; }
        public string vserver { get; set; }
        public string vserver_val { get; set; }

        [JsonProperty("v-zone")]
        public string vzone { get; set; }

        [JsonProperty("v-zone_val")]
        public string vzone_val { get; set; }
        public string service { get; set; }
        public string service_val { get; set; }

        [JsonProperty("proxy-protocol")]
        public string proxyprotocol { get; set; }

        [JsonProperty("proxy-protocol_val")]
        public string proxyprotocol_val { get; set; }

        [JsonProperty("use-proxy-protocol-addr")]
        public string useproxyprotocoladdr { get; set; }

        [JsonProperty("use-proxy-protocol-addr_val")]
        public string useproxyprotocoladdr_val { get; set; }

        [JsonProperty("ftp-protection-profile")]
        public string ftpprotectionprofile { get; set; }

        [JsonProperty("ftp-protection-profile_val")]
        public string ftpprotectionprofile_val { get; set; }

        [JsonProperty("web-protection-profile")]
        public string webprotectionprofile { get; set; }

        [JsonProperty("web-protection-profile_val")]
        public string webprotectionprofile_val { get; set; }
        public string replacemsg { get; set; }
        public string replacemsg_val { get; set; }

        [JsonProperty("server-pool")]
        public string serverpool { get; set; }

        [JsonProperty("server-pool_val")]
        public string serverpool_val { get; set; }

        [JsonProperty("traffic-mirror")]
        public string trafficmirror { get; set; }

        [JsonProperty("traffic-mirror_val")]
        public string trafficmirror_val { get; set; }

        [JsonProperty("traffic-mirror-profile")]
        public string trafficmirrorprofile { get; set; }

        [JsonProperty("traffic-mirror-profile_val")]
        public string trafficmirrorprofile_val { get; set; }

        [JsonProperty("traffic-mirror-type")]
        public string trafficmirrortype { get; set; }

        [JsonProperty("traffic-mirror-type_val")]
        public string trafficmirrortype_val { get; set; }

        [JsonProperty("allow-hosts")]
        public string allowhosts { get; set; }

        [JsonProperty("allow-hosts_val")]
        public string allowhosts_val { get; set; }

        [JsonProperty("allow-list")]
        public string allowlist { get; set; }

        [JsonProperty("allow-list_val")]
        public string allowlist_val { get; set; }

        [JsonProperty("acceleration-policy")]
        public string accelerationpolicy { get; set; }

        [JsonProperty("acceleration-policy_val")]
        public string accelerationpolicy_val { get; set; }

        [JsonProperty("https-service")]
        public string httpsservice { get; set; }

        [JsonProperty("https-service_val")]
        public string httpsservice_val { get; set; }

        [JsonProperty("multi-certificate")]
        public string multicertificate { get; set; }

        [JsonProperty("multi-certificate_val")]
        public string multicertificate_val { get; set; }

        [JsonProperty("adfs-certificate-service")]
        public string adfscertificateservice { get; set; }

        [JsonProperty("adfs-certificate-service_val")]
        public string adfscertificateservice_val { get; set; }

        [JsonProperty("adfs-certificate-ssl-client-verify")]
        public string adfscertificatesslclientverify { get; set; }

        [JsonProperty("adfs-certificate-ssl-client-verify_val")]
        public string adfscertificatesslclientverify_val { get; set; }

        [JsonProperty("send-buffers-number")]
        public int sendbuffersnumber { get; set; }

        [JsonProperty("reply-100-continue")]
        public string reply100continue { get; set; }

        [JsonProperty("reply-100-continue_val")]
        public string reply100continue_val { get; set; }

        [JsonProperty("forward-expect-100-continue")]
        public string forwardexpect100continue { get; set; }

        [JsonProperty("forward-expect-100-continue_val")]
        public string forwardexpect100continue_val { get; set; }

        [JsonProperty("transaction-based-persistence")]
        public string transactionbasedpersistence { get; set; }

        [JsonProperty("transaction-based-persistence_val")]
        public string transactionbasedpersistence_val { get; set; }

        [JsonProperty("certificate-type")]
        public string certificatetype { get; set; }

        [JsonProperty("certificate-type_val")]
        public string certificatetype_val { get; set; }

        [JsonProperty("lets-certificate")]
        public string letscertificate { get; set; }

        [JsonProperty("lets-certificate_val")]
        public string letscertificate_val { get; set; }
        public string certificate { get; set; }
        public string certificate_val { get; set; }

        [JsonProperty("certificate-group")]
        public string certificategroup { get; set; }

        [JsonProperty("certificate-group_val")]
        public string certificategroup_val { get; set; }

        [JsonProperty("intermediate-certificate-group")]
        public string intermediatecertificategroup { get; set; }

        [JsonProperty("intermediate-certificate-group_val")]
        public string intermediatecertificategroup_val { get; set; }

        [JsonProperty("ssl-client-verify")]
        public string sslclientverify { get; set; }

        [JsonProperty("ssl-client-verify_val")]
        public string sslclientverify_val { get; set; }

        [JsonProperty("use-ciphers-group")]
        public string useciphersgroup { get; set; }

        [JsonProperty("use-ciphers-group_val")]
        public string useciphersgroup_val { get; set; }

        [JsonProperty("ssl-ciphers-group")]
        public string sslciphersgroup { get; set; }

        [JsonProperty("ssl-ciphers-group_val")]
        public string sslciphersgroup_val { get; set; }

        [JsonProperty("tls-v10")]
        public string tlsv10 { get; set; }

        [JsonProperty("tls-v10_val")]
        public string tlsv10_val { get; set; }

        [JsonProperty("tls-v11")]
        public string tlsv11 { get; set; }

        [JsonProperty("tls-v11_val")]
        public string tlsv11_val { get; set; }

        [JsonProperty("tls-v12")]
        public string tlsv12 { get; set; }

        [JsonProperty("tls-v12_val")]
        public string tlsv12_val { get; set; }

        [JsonProperty("tls-v13")]
        public string tlsv13 { get; set; }

        [JsonProperty("tls-v13_val")]
        public string tlsv13_val { get; set; }

        [JsonProperty("ssl-noreg")]
        public string sslnoreg { get; set; }

        [JsonProperty("ssl-noreg_val")]
        public string sslnoreg_val { get; set; }

        [JsonProperty("ssl-cipher")]
        public string sslcipher { get; set; }

        [JsonProperty("ssl-cipher_val")]
        public string sslcipher_val { get; set; }

        [JsonProperty("ssl-custom-cipher")]
        public string sslcustomcipher { get; set; }

        [JsonProperty("ssl-custom-cipher_val")]
        public string sslcustomcipher_val { get; set; }

        [JsonProperty("tls13-custom-cipher")]
        public string tls13customcipher { get; set; }

        [JsonProperty("tls13-custom-cipher_val")]
        public string tls13customcipher_val { get; set; }

        [JsonProperty("rfc7919-comply")]
        public string rfc7919comply { get; set; }

        [JsonProperty("rfc7919-comply_val")]
        public string rfc7919comply_val { get; set; }

        [JsonProperty("supported-groups")]
        public string supportedgroups { get; set; }

        [JsonProperty("supported-groups_val")]
        public string supportedgroups_val { get; set; }
        public string sni { get; set; }
        public string sni_val { get; set; }

        [JsonProperty("sni-certificate")]
        public string snicertificate { get; set; }

        [JsonProperty("sni-certificate_val")]
        public string snicertificate_val { get; set; }

        [JsonProperty("sni-strict")]
        public string snistrict { get; set; }

        [JsonProperty("sni-strict_val")]
        public string snistrict_val { get; set; }
        public string urlcert { get; set; }
        public string urlcert_val { get; set; }

        [JsonProperty("urlcert-group")]
        public string urlcertgroup { get; set; }

        [JsonProperty("urlcert-group_val")]
        public string urlcertgroup_val { get; set; }

        [JsonProperty("urlcert-hlen")]
        public int urlcerthlen { get; set; }

        [JsonProperty("case-sensitive")]
        public string casesensitive { get; set; }

        [JsonProperty("case-sensitive_val")]
        public string casesensitive_val { get; set; }
        public string status { get; set; }
        public string status_val { get; set; }
        public string comment { get; set; }

        [JsonProperty("block-port")]
        public string blockport { get; set; }

        [JsonProperty("block-port_val")]
        public string blockport_val { get; set; }
        public string noparse { get; set; }
        public string noparse_val { get; set; }

        [JsonProperty("data-capture-port")]
        public string datacaptureport { get; set; }

        [JsonProperty("data-capture-port_val")]
        public string datacaptureport_val { get; set; }

        [JsonProperty("monitor-mode")]
        public string monitormode { get; set; }

        [JsonProperty("monitor-mode_val")]
        public string monitormode_val { get; set; }

        [JsonProperty("web-cache")]
        public string webcache { get; set; }

        [JsonProperty("web-cache_val")]
        public string webcache_val { get; set; }

        [JsonProperty("http-to-https")]
        public string httptohttps { get; set; }

        [JsonProperty("http-to-https_val")]
        public string httptohttps_val { get; set; }

        [JsonProperty("redirect-naked-domain")]
        public string redirectnakeddomain { get; set; }

        [JsonProperty("redirect-naked-domain_val")]
        public string redirectnakeddomain_val { get; set; }

        [JsonProperty("sessioncookie-enforce")]
        public string sessioncookieenforce { get; set; }

        [JsonProperty("sessioncookie-enforce_val")]
        public string sessioncookieenforce_val { get; set; }
        public string syncookie { get; set; }
        public string syncookie_val { get; set; }

        [JsonProperty("half-open-threshold")]
        public int halfopenthreshold { get; set; }

        [JsonProperty("client-certificate-forwarding")]
        public string clientcertificateforwarding { get; set; }

        [JsonProperty("client-certificate-forwarding_val")]
        public string clientcertificateforwarding_val { get; set; }

        [JsonProperty("client-certificate-forwarding-sub-header")]
        public string clientcertificateforwardingsubheader { get; set; }

        [JsonProperty("client-certificate-forwarding-cert-header")]
        public string clientcertificateforwardingcertheader { get; set; }

        [JsonProperty("http-pipeline")]
        public string httppipeline { get; set; }

        [JsonProperty("http-pipeline_val")]
        public string httppipeline_val { get; set; }

        [JsonProperty("hsts-header")]
        public string hstsheader { get; set; }

        [JsonProperty("hsts-header_val")]
        public string hstsheader_val { get; set; }

        [JsonProperty("hsts-max-age")]
        public int hstsmaxage { get; set; }

        [JsonProperty("hsts-include-subdomains")]
        public string hstsincludesubdomains { get; set; }

        [JsonProperty("hsts-include-subdomains_val")]
        public string hstsincludesubdomains_val { get; set; }

        [JsonProperty("hsts-preload")]
        public string hstspreload { get; set; }

        [JsonProperty("hsts-preload_val")]
        public string hstspreload_val { get; set; }

        [JsonProperty("hpkp-header")]
        public string hpkpheader { get; set; }

        [JsonProperty("hpkp-header_val")]
        public string hpkpheader_val { get; set; }

        [JsonProperty("prefer-current-session")]
        public string prefercurrentsession { get; set; }

        [JsonProperty("prefer-current-session_val")]
        public string prefercurrentsession_val { get; set; }

        [JsonProperty("policy-id")]
        public string policyid { get; set; }
        public int sub_table_id { get; set; }
        public string sub_table_action { get; set; }
        public string sub_table_action_val { get; set; }

        [JsonProperty("sz_http-content-routing-list")]
        public int sz_httpcontentroutinglist { get; set; }

        [JsonProperty("client-real-ip")]
        public string clientrealip { get; set; }

        [JsonProperty("client-real-ip_val")]
        public string clientrealip_val { get; set; }

        [JsonProperty("real-ip-addr")]
        public string realipaddr { get; set; }

        [JsonProperty("client-real-ip-random-port")]
        public string clientrealiprandomport { get; set; }

        [JsonProperty("client-real-ip-random-port_val")]
        public string clientrealiprandomport_val { get; set; }
        public string http2 { get; set; }
        public string http2_val { get; set; }

        [JsonProperty("tcp-recv-timeout")]
        public int tcprecvtimeout { get; set; }

        [JsonProperty("http-header-timeout")]
        public int httpheadertimeout { get; set; }

        [JsonProperty("tcp-conn-timeout")]
        public int tcpconntimeout { get; set; }

        [JsonProperty("internal-cookie-httponly")]
        public string internalcookiehttponly { get; set; }

        [JsonProperty("internal-cookie-httponly_val")]
        public string internalcookiehttponly_val { get; set; }

        [JsonProperty("internal-cookie-secure")]
        public string internalcookiesecure { get; set; }

        [JsonProperty("internal-cookie-secure_val")]
        public string internalcookiesecure_val { get; set; }

        [JsonProperty("internal-cookie-samesite")]
        public string internalcookiesamesite { get; set; }

        [JsonProperty("internal-cookie-samesite_val")]
        public string internalcookiesamesite_val { get; set; }

        [JsonProperty("internal-cookie-samesite-value")]
        public string internalcookiesamesitevalue { get; set; }

        [JsonProperty("internal-cookie-samesite-value_val")]
        public string internalcookiesamesitevalue_val { get; set; }

        [JsonProperty("content-security-policy-inline")]
        public string contentsecuritypolicyinline { get; set; }

        [JsonProperty("content-security-policy-inline_val")]
        public string contentsecuritypolicyinline_val { get; set; }

        [JsonProperty("ssl-quiet-shutdown")]
        public string sslquietshutdown { get; set; }

        [JsonProperty("ssl-quiet-shutdown_val")]
        public string sslquietshutdown_val { get; set; }

        [JsonProperty("ssl-session-timeout")]
        public int sslsessiontimeout { get; set; }

        [JsonProperty("client-timeout")]
        public int clienttimeout { get; set; }

        [JsonProperty("retry-on")]
        public string retryon { get; set; }

        [JsonProperty("retry-on_val")]
        public string retryon_val { get; set; }

        [JsonProperty("retry-on-cache-size")]
        public int retryoncachesize { get; set; }

        [JsonProperty("retry-on-connect-failure")]
        public string retryonconnectfailure { get; set; }

        [JsonProperty("retry-on-connect-failure_val")]
        public string retryonconnectfailure_val { get; set; }

        [JsonProperty("retry-times-on-connect-failure")]
        public int retrytimesonconnectfailure { get; set; }

        [JsonProperty("retry-on-http-layer")]
        public string retryonhttplayer { get; set; }

        [JsonProperty("retry-on-http-layer_val")]
        public string retryonhttplayer_val { get; set; }

        [JsonProperty("retry-times-on-http-layer")]
        public int retrytimesonhttplayer { get; set; }

        [JsonProperty("retry-on-http-response-codes")]
        public string retryonhttpresponsecodes { get; set; }

        [JsonProperty("retry-on-http-response-codes_val")]
        public string retryonhttpresponsecodes_val { get; set; }

        [JsonProperty("replacemsg-on-connect-failure")]
        public string replacemsgonconnectfailure { get; set; }

        [JsonProperty("replacemsg-on-connect-failure_val")]
        public string replacemsgonconnectfailure_val { get; set; }

        [JsonProperty("chunk-encoding")]
        public string chunkencoding { get; set; }

        [JsonProperty("chunk-encoding_val")]
        public string chunkencoding_val { get; set; }

        [JsonProperty("payload-based-content-type")]
        public string payloadbasedcontenttype { get; set; }

        [JsonProperty("payload-based-content-type_val")]
        public string payloadbasedcontenttype_val { get; set; }
        public string tlog { get; set; }
        public string tlog_val { get; set; }

        [JsonProperty("web-cache-storage")]
        public string webcachestorage { get; set; }

        [JsonProperty("web-cache-storage_val")]
        public string webcachestorage_val { get; set; }
        public string scripting { get; set; }
        public string scripting_val { get; set; }

        [JsonProperty("scripting-list")]
        public string scriptinglist { get; set; }

        [JsonProperty("ztna-profile")]
        public string ztnaprofile { get; set; }

        [JsonProperty("ztna-profile_val")]
        public string ztnaprofile_val { get; set; }
        public string tags { get; set; }

        [JsonProperty("http-parse-max-size")]
        public int httpparsemaxsize { get; set; }
    }

    public class SetBindingsResponse
    {
        public Results results { get; set; }
    }


}
