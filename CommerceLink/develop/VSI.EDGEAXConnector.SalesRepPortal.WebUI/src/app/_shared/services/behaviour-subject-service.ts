import { Injectable } from '@angular/core'; 
import { Observable } from 'rxjs';  
import { BehaviorSubject} from 'rxjs'; 
 
@Injectable() export class BehaviourSubjectService { 
 
    private behave = new BehaviorSubject<Object>({});
    private behave1 = new BehaviorSubject<Object>({});
    
    constructor( ) { } 
    
    setBehaviorView(behave: Object) { 
        this.behave.next(behave); 
    }

    getBehaviorView(): Observable<any> { 
        return this.behave.asObservable(); 
    }


    setQuoteStatus(behave1: Object) { 
        this.behave1.next(behave1); 
    }

    getQuoteStatus(): Observable<any> { 
        return this.behave1.asObservable(); 
    }
}