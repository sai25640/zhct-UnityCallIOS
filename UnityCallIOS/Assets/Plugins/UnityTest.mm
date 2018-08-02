//
//  UnityTest.m
//  Unity-iPhone
//
//  Created by Leon on 2018/7/27.
//

#import <Foundation/Foundation.h>
#import "UnityTest.h"
#import <MobileCoreServices/MobileCoreServices.h>
#import <AssetsLibrary/AssetsLibrary.h>

@implementation UnityTest

-(void)OpenTarget:(UIImagePickerControllerSourceType) type{
    UIImagePickerController *picker;
    picker = [[UIImagePickerController alloc]init];
    picker.delegate = self;
    picker.allowsEditing = NO;
    picker.sourceType = type;
    picker.mediaTypes = @[@"public.movie"];
    NSLog(@"%@",picker.mediaTypes);
    
    [self presentViewController: picker animated:YES completion:nil];
}

-(void)imagePickerController:(UIImagePickerController *)pic didFinishPickingMediaWithInfo:(NSDictionary *)
info{
    [pic dismissViewControllerAnimated:YES completion:nil];
    NSString *mediaType=[info objectForKey:UIImagePickerControllerMediaType];
    if ([mediaType isEqualToString:@"public.image"]) {//如果是拍照
        UIImage *image = info[UIImagePickerControllerOriginalImage];
        
        NSString *imagePath =[self GetSavePath:@"temp.mp4"];
        [self SaveFileToDoc: image path:imagePath];
    }else if([mediaType isEqualToString:@"public.movie"]){
        NSURL * url = [info objectForKey:UIImagePickerControllerMediaURL];
        NSData * data = [NSData dataWithContentsOfURL:url];
        NSLog(@"%@",data);
        
        
        NSString *videoPath =[self GetSavePath:@"temp.mp4"];
        NSString *videoStr = [data base64Encoding];
        UnitySendMessage("Main Camera", "IOSBack", videoStr.UTF8String);
    }
    
}

-(NSString *)GetSavePath:(NSString *)filename{
    NSArray *pathArray = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
    NSString *docPath =[pathArray objectAtIndex:0];
    return [docPath stringByAppendingPathComponent:filename];
}

-(void)SaveFileToDoc:(UIImage *)image path:(NSString *)path{
    NSData *data;
    
    if (UIImagePNGRepresentation(image)==nil) {
        data = UIImageJPEGRepresentation(image, 1);
    }else{
        data = UIImagePNGRepresentation(image);
    }
    
    NSString *imageStr = [data base64Encoding];
    UnitySendMessage("Main Camera", "IOSBack", imageStr.UTF8String);
    //[data writeToFile:path atomically:YES];
    //UnitySendMessage("Main Camera", "IOSBack", "Temp.jpg");
}

-(void)imagePickerControllerDidCancel:(UIImagePickerController *)picker{
    UnitySendMessage("Main Camera", "IOSBack", "");
    [picker dismissViewControllerAnimated:YES completion:nil];
    
}

extern "C"
{
    void IOS_OpenCamera()
    {
        UnityTest *app=[[UnityTest alloc] init];
        UIViewController *vc = UnityGetGLViewController();
        [vc.view addSubview:app.view];
        
        [app OpenTarget:UIImagePickerControllerSourceTypeCamera];
    }
    
    void IOS_OpenAlbum()
    {
        UnityTest *app =[[UnityTest alloc] init];
        UIViewController *vc =UnityGetGLViewController();
        [vc.view addSubview:app.view];
        
        [app OpenTarget:UIImagePickerControllerSourceTypePhotoLibrary];
    }

}
@end
