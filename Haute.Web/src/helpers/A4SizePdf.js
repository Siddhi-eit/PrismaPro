module.exports = `<!DOCTYPE html>

<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
   <head>
      <meta charset="utf-8" />
      <title></title>
   </head>
   <body style="background:#e0e0e0;margin:0;font-family:Tahoma,sans-serif;line-height:20px;box-sizing:border-box;">
      <div style="width:297mm;background:url(http://createyourformula.hautecustombeauty.com/assets/img/pdf/hcbmdfl_logo.png) repeat;height:210mm;box-shadow:0 0.5mm 2mm rgb(0 0 0 / 30%);margin:5mm;overflow:hidden;position:relative;box-sizing: border-box;page-break-after:always;">
         <section style="padding:10mm 10px 0px 10mm;box-sizing:border-box;">
            <div>
               <div style="height:32mm;border:1px solid #5c5c5c;border-radius:20px;padding:15px 0px 17px 25px;margin:18px 0px;box-sizing:border-box;">
                  <div style="border:5px solid #000000;background:#ffffff;height:100%;width:93mm;box-sizing:border-box;">
                     <div style="display:flex;flex-wrap:wrap;box-sizing:border-box;">
                       <div style="vertical-align:middle;text-align:center;font-size:40px;line-height:50px;padding:14px 12px 14px 12px;height:100%;box-sizing:border-box;"><img src="{#SANITIZEDQRCODE}"/></div>
                        <div style="font-size:13px;line-height:17px;width:80%;margin-top:5px;color:#000;box-sizing:border-box;">
                          <dl style="margin:0px;">
                              <dt style="float:left;clear:left;margin-right:5px;box-sizing:border-box;">DATE SANITIZED:</dt>
                              <dd style="
                                 margin-left: 0px;
                                 ">{#DATESANITIZED}</dd>
                                 <dt style="float:left;clear:left;margin-right:5px;box-sizing:border-box;">CANISTER REF TRACKING NO:</dt>
                                 <dd style="margin-left:0px;box-sizing:border-box;">{#CANISTERREFTRACKINGNO}</dd>
                                 <dt style="float:left;clear:left;margin-right:5px;box-sizing:border-box;">PRODUCT:</dt>
                                 <dd style="margin-left:0px;box-sizing:border-box;">XXXXXXXXXXXXXXXX</dd>
                                 <dt style="float:left;clear:left;margin-right:5px;box-sizing:border-box;">>SECURITY CLEARED BY:</dt>
                                 <dd style="margin-left:0px;box-sizing:border-box;">XXXXXXXXXXXXXXXX</dd>
                           </dl>
                        </div>
                     </div>
                  </div>
               </div>
            </div>
         </section>
      </div>
   </body>
</html>`;