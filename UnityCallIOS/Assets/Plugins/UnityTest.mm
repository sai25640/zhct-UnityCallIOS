//
//  UnityTest.m
//  Unity-iPhone
//
//  Created by Leon on 2018/7/27.
//

#import <Foundation/Foundation.h>
#import "UnityTest.h"

@implementation UnityTest

-(void)OpenTarget:(UIImagePickerControllerSourceType) type{
    UIImagePickerController *picker;
    picker = [[UIImagePickerController alloc]init];
    picker.delegate = self;
    picker.allowsEditing = YES;
    picker.sourceType = type;
    
    [self presentViewController: picker animated:YES completion:nil];
}

-(void)imagePickerController:(UIImagePickerController *)pic didFinishPickingMediaWithInfo:(NSDictionary *)
info{
    [pic dismissViewControllerAnimated:YES completion:nil];
    
    UIImage *image = info[UIImagePickerControllerOriginalImage];
    
    NSString *imagePath =[self GetSavePath:@"Temp.jpg"];
    [self SaveFileToDoc: image path:imagePath];
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
    
    NSString *imageStr = [data bas64Encoding];
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
