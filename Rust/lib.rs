pub fn transform(input: &str, line_width: u32) -> String {
let mut result_str = String::new();//output string
let symbol:char = ' ';//to see how it goes

//check if input variables are correct
if input.is_empty() || line_width == 0{
    return result_str;
}

let input_string = input.to_string();
let words:Vec<&str>= input_string.split(" ").collect();//split string to words
let words_count = words.len();

let mut append_words:Vec<String> = Vec::with_capacity(words_count);
append_words.resize(words_count,String::new());

//convert to string each word and check the length of each word
for j in 0..words_count{
        if (words[j].len() as u32) > line_width{
            std::mem::forget(result_str); 
            std::mem::forget(append_words);
            std::mem::forget(words);
            return "One or a few words are bigger than line width! Correct your variables!".to_string();
        }
   append_words[j] = words[j].to_string(); 
}

let mut i:usize=0;//counter

while i<words_count{
    let mut str_buff = String::new();// string buffer
    let mut append = false;//condition to append
    let mut words_in_string= i + 1;//amount of words in string
    let mut str_length = append_words[i].len() as u32;//length of string buffer
    
    //check if out of range
    while (words_in_string as i32)<(words_count as i32){
    //check before appending
     if str_length<(line_width - (append_words[words_in_string].len() as u32)){
        append = true;
        str_length += append_words[words_in_string].len() as u32;
        words_in_string = words_in_string+1;
        continue;
        }
    break;
    }
    
    words_in_string = words_in_string-i;
    let mut amount_of_spaces = (line_width as i32) - (str_length as i32);
    
    //fill spaces till we can
    if append{
    while amount_of_spaces>0{
        for index in 0..words_in_string-1{
        if amount_of_spaces>0{
            append_words[index+i].push(symbol);
            amount_of_spaces-=1;
            }
        else{
            break;
            }    
        }
    }
    
    //transfer words to a buffer
    for indexer in 0..words_in_string{
        str_buff.push_str(&append_words[indexer+i]);
    } 
    }
    //if its single word
    if !append{
        while str_length<line_width {
          str_length+=1;
          append_words[i].push(symbol);  
        }
        str_buff.push_str(&append_words[i]);
    }
    
    i = i + words_in_string;//transfer index
    result_str.push_str(&str_buff);

    //excluse one word message and last word
    if (words_count>1)&&(i<words_count){
    result_str.push('\n');    
    }

    }
std::mem::forget(append_words);
std::mem::forget(words);

return result_str;
}

#[cfg(test)]
mod tests {
    use super::transform;

    #[test]
    fn simple() {
        let test_cases = [
            ("", 5, ""),
            ("test", 5, "test "),
            ("Lorem ipsum dolor sit amet consectetur adipiscing elit sed do eiusmod tempor incididunt ut labore et dolore magna aliqua", 12,
             "Lorem  ipsum\ndolor    sit\namet        \nconsectetur \nadipiscing  \nelit  sed do\neiusmod     \ntempor      \nincididunt  \nut labore et\ndolore magna\naliqua      ")
        ];

        for &(input, line_width, expected) in &test_cases {
            println!("input: '{}'", input);
            assert_eq!(transform(input, line_width), expected);
        }
    }
}